using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using Serilog;
using SpaceCards.API;
using SpaceCards.BusinessLogic;
using SpaceCards.DataAccess.Postgre;
using SpaceCards.Domain;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Logging.AddSerilog(logger);

var serviceName = "SpaceCards.API";

var servieceVersion = "1.0.0";

builder.Services.AddOpenTelemetryTracing(builder =>
{
    builder
    .AddJaegerExporter()
    .AddSource(serviceName)
    .SetResourceBuilder(
        ResourceBuilder.CreateDefault()
        .AddService(serviceName: serviceName, serviceVersion: servieceVersion))
    .AddHttpClientInstrumentation()
    .AddAspNetCoreInstrumentation()
    .AddEntityFrameworkCoreInstrumentation();
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

app.UseCors(x =>
{
    x.WithHeaders().AllowAnyHeader();
    x.WithOrigins().AllowAnyOrigin();
    x.AllowAnyMethod();
});

app.MapControllers();

app.Run();

public partial class Program { }