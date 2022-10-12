using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using SpaceCards.API;
using SpaceCards.API.Options;
using SpaceCards.BusinessLogic;
using SpaceCards.DataAccess.Postgre;
using SpaceCards.DataAccess.Postgre.Repositories;
using SpaceCards.Domain.Interfaces;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(BaseSchema.NAME)
    .AddScheme<CustomBearerAuthenticationOption, CustomBearerAuthenticationHandler>(
    BaseSchema.NAME, options => { });

builder.Services.Configure<JWTSecretOptions>(
    builder.Configuration.GetSection(JWTSecretOptions.JWTSecret));

builder.Services.AddLogging(b =>
{
    var logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    b.AddSerilog(logger, true);
});

var serviceName = "SpaceCards.API";

var servieceVersion = "1.0.0";

builder.Services.AddOpenTelemetryTracing(builder =>
{
    builder
        .AddJaegerExporter(options =>
        {
            options.AgentHost = "jaeger";
        })
        .AddSource(serviceName)
        .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
            .AddService(serviceName: serviceName, serviceVersion: servieceVersion))
        .AddHttpClientInstrumentation()
        .AddAspNetCoreInstrumentation()
        .AddEntityFrameworkCoreInstrumentation(options =>
        {
            options.SetDbStatementForText = true;
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

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<ApiMappingProfile>();
    cfg.AddProfile<DataAccessMappingProfile>();
});

builder.Services.AddScoped<ICardsRepository, CardsRepository>();

builder.Services.AddScoped<ICardsService, CardsService>();

builder.Services.AddScoped<IGroupsRepository, GroupsRepository>();

builder.Services.AddScoped<IGroupsService, GroupsService>();

builder.Services.AddScoped<IGuessedCardRepository, GuessedCardRepository>();

builder.Services.AddScoped<IGuessedCardsService, GuessedCardsService>();

builder.Services.AddScoped<IStatisticsRepository, StatisticsRepository>();

builder.Services.AddScoped<IStatisticsService, StatisticsService>();

builder.Services.AddScoped<IUsersAccountRepository, UsersAccountRepository>();

builder.Services.AddScoped<IUsersAccountService, UsersAccountService>();

builder.Services.AddScoped<SecurityService>();

builder.Services.AddScoped<ISessionsRepository, SessionsRepository>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    options.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins().AllowAnyOrigin();
    x.AllowAnyMethod();
});

app.MapControllers();

app.Run();

public partial class Program { }