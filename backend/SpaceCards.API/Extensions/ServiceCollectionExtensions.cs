using AspNet.Security.OAuth.MailRu;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.Net.Http.Headers;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;
using SpaceCards.API.Options;
using SpaceCards.BusinessLogic;
using SpaceCards.DataAccess.Postgre.Repositories;
using SpaceCards.Domain.Interfaces;

namespace SpaceCards.API.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<ICardsRepository, CardsRepository>();
            services.AddScoped<IGroupsRepository, GroupsRepository>();
            services.AddScoped<IGuessedCardRepository, GuessedCardRepository>();
            services.AddScoped<IStatisticsRepository, StatisticsRepository>();
            services.AddScoped<IUsersAccountRepository, UsersAccountRepository>();
            services.AddScoped<ISessionsRepository, SessionsRepository>();
            services.AddScoped<IOAuthUsersRepository, OAuthUsersRepository>();
            services.AddScoped<IOAuthUserTokensRepository, OAuthUserTokensRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            services.AddScoped<ICardsService, CardsService>();
            services.AddScoped<IGroupsService, GroupsService>();
            services.AddScoped<IGuessedCardsService, GuessedCardsService>();
            services.AddScoped<IStatisticsService, StatisticsService>();
            services.AddScoped<IUsersAccountService, UsersAccountService>();
            services.AddScoped<SecurityService>();
            services.AddScoped<IOAuthUsersService, OAuthUsersService>();
            services.AddScoped<IOAuthUserTokensService, OAuthUserTokensService>();

            return services;
        }

        public static WebApplicationBuilder AddApiOptions(this WebApplicationBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.Configure<JWTSecretOptions>(
                builder.Configuration.GetSection(JWTSecretOptions.JWTSecret));

            builder.Services.Configure<ExternalAuthenticationOptions>(
                builder.Configuration.GetSection(nameof(ExternalAuthenticationOptions)));

            return builder;
        }

        public static WebApplicationBuilder AddApiAuthentication(this WebApplicationBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentException(nameof(builder));
            }

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = "JWT_OR_OAUTH";
                options.DefaultChallengeScheme = "JWT_OR_OAUTH";
            })
            .AddJwtBearer()
            .AddScheme<CustomBearerAuthenticationOption, CustomBearerAuthenticationHandler>(
                BaseSchema.NAME, options => { })
            .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, o =>
            {
                o.Cookie.Name = "_sp_i_i";
            })
            .AddGoogle(GoogleDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = builder.Configuration
                    .GetSection(nameof(ExternalAuthenticationOptions))
                    .Get<ExternalAuthenticationOptions>().Google.ClientId;

                options.ClientSecret = builder.Configuration
                    .GetSection(nameof(ExternalAuthenticationOptions))
                    .Get<ExternalAuthenticationOptions>().Google.ClientSecret;
                options.AccessType = "offline";
                options.ClaimActions.MapJsonKey("image", "picture");
                options.SaveTokens = true;
            })
            .AddMailRu(MailRuAuthenticationDefaults.AuthenticationScheme, options =>
            {
                options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ClientId = builder.Configuration
                    .GetSection(nameof(ExternalAuthenticationOptions))
                    .Get<ExternalAuthenticationOptions>().MailRu.ClientId;

                options.ClientSecret = builder.Configuration
                    .GetSection(nameof(ExternalAuthenticationOptions))
                    .Get<ExternalAuthenticationOptions>().MailRu.ClientSecret;

                options.ClaimActions.MapJsonKey("image", "image");
                options.SaveTokens = true;
            })
            .AddPolicyScheme("JWT_OR_OAUTH", "JWT_OR_OAUTH", options =>
            {
                options.ForwardDefaultSelector = context =>
                {
                    string auth = context.Request.Headers[HeaderNames.Authorization];
                    if (!string.IsNullOrEmpty(auth) && auth.StartsWith("Bearer "))
                        return BaseSchema.NAME;

                    return CookieAuthenticationDefaults.AuthenticationScheme;
                };
            });

            return builder;
        }

        public static IServiceCollection AddJaegerTracing(this IServiceCollection services)
        {
            if (services is null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            var serviceName = "SpaceCards.API";
            var servieceVersion = "1.0.0";

            services.AddOpenTelemetryTracing(builder =>
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

            return services;
        }
    }
}
