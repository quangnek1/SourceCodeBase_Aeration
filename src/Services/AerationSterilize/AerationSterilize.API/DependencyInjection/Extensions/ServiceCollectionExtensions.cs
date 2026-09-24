using AerationSterilize.Application.DependencyInjection.Extensions;
using Contracts.Identity;
using Infrastructure.Identity;

namespace AerationSterilize.API.DependencyInjection.Extensions;

public static class ServiceCollectionExtensions
{
    public static void AddApiVersioningAPI(this IServiceCollection services)
       => services.AddApiVersioning(options => options.ReportApiVersions = true)
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

    public static void ConfigureCors(this IServiceCollection services, IConfiguration configuration)
    {
        var origins = configuration["AllowOrigins"];

        services.AddTransient<ITokenService, TokenService>();
        services.AddConfigureJWTAuthentication(configuration);
        services.AddCors(option =>
        {
            option.AddPolicy("CorsPolicy", buider =>
            {
                if (origins == "*")
                    buider.SetIsOriginAllowed(_ => true);
                else
                    buider.WithOrigins(origins ?? string.Empty);

                buider.AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
    }
   

}

