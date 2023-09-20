using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using OpenTelemetry.Metrics;
using Serilog;
using SpaceCards.API;
using SpaceCards.API.Extensions;
using SpaceCards.API.Options;
using SpaceCards.API.Services.JwtService;
using SpaceCards.DataAccess.Postgre;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.AddApiOptions();

builder.AddApiAuthentication();

builder.Services.AddLogging(b =>
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    b.AddSerilog(logger, true);
});

builder.Services.AddJaegerTracing();

builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowSpaceCardsApp", p =>
    {
        p.WithOrigins("http://localhost:3000")
        .WithHeaders().AllowAnyHeader()
        .WithMethods().AllowAnyMethod()
        .AllowCredentials()
        .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

builder.Services.AddOpenTelemetryMetrics(builder =>
{
    builder.AddPrometheusExporter(options =>
    {
        options.StartHttpListener = true;
        options.HttpListenerPrefixes = new[] { "http://prometheus:9090" };
        options.ScrapeResponseCacheDurationMilliseconds = 0;
    });
});

// Add services to the container.
builder.Services.AddDbContext<SpaceCardsDbContext>(options =>
{
    options.UseNpgsql(builder.Configuration.GetConnectionString("SpaceCardsDb"));
});

//builder.Services.AddStackExchangeRedisCache(options =>
//{
//    options.Configuration = builder
//        .Configuration
//        .GetSection(nameof(RedisOptions))
//        .Get<RedisOptions>()
//        .ConnectionString;
//});

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApiMappingProfile>();
    cfg.AddProfile<DataAccessMappingProfile>();
});

builder.Services.AddRepositories();

builder.Services.AddServices();

builder.Services.AddJwtService(options =>
{
    options.Provider = new JwtNetProvider();
    options.Secret = builder.Configuration
                    .GetSection(nameof(JWTSecretOptions.JWTSecret))
                    .Get<JWTSecretOptions>().Secret;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

//builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowSpaceCardsApp");

app.UseJwtFromCookie();

app.UseAuthentication();

app.UseAuthorization();

//app.UseCors(x =>
//{
//    x.WithHeaders().AllowAnyHeader();
//    x.WithOrigins().AllowAnyOrigin();
//    x.AllowAnyMethod();
//    x.AllowCredentials();
//});

app.MapControllers();

app.Run();

public partial class Program { }