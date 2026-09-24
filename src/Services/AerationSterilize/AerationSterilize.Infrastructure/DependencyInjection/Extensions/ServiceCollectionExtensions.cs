using AerationSterilize.Application.Abstractions;
using AerationSterilize.Infrastructure.Search;
using AerationSterilize.Infrastructure.Services;
using AerationSterilize.Infrastructure.Services.Users;
using Contracts.BackgroundJobs;
using Contracts.Services.Cache;
using Contracts.Services.Excel;
using Contracts.Services.PDF;
using Contracts.Services.QRCode;
using Elastic.Clients.Elasticsearch;
using Elastic.Transport;
using Hangfire;
using Hangfire.SqlServer;
using Infrastructure.Extensions;
using Infrastructure.Services.BackgroundJobs;
using Infrastructure.Services.Cache;
using Infrastructure.Services.Excel;
using Infrastructure.Services.PDF;
using Infrastructure.Services.QRCode;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Options;

namespace AerationSterilize.Infrastructure.DependencyInjection.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureRedis();
        services.ConfigureHangfire(configuration);
        services.ConfigureElasticsearch(configuration);

        services.AddScoped<IExcelReaderService, EpplusExcelReaderService>();
        services.AddSingleton<IPdfService, PdfService>();
        services.AddSingleton<ITemplateRenderer, RazorTemplateRenderer>();
        services.AddSingleton<IQrCodeServices, QrCodeServices>();
        services.AddScoped<ICacheService, CacheService>();

        services.AddScoped<IScheduledJobService, HangfireService>();
        services.AddScoped<IAerationScheduledJob, AerationScheduledJobService>();
        services.AddScoped<ISignalRServices, SignalRService>();

        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        return services;
    }

    private static void ConfigureElasticsearch(this IServiceCollection services, IConfiguration configuration)
    {
        var options = configuration.GetSection(nameof(ElasticsearchOptions)).Get<ElasticsearchOptions>() ?? new ElasticsearchOptions();

        services.AddSingleton(options);
        services.AddSingleton(_ =>
        {
            var settings = new ElasticsearchClientSettings(new Uri(options.Uri))
                .DefaultIndex(options.DefaultIndex);

            if (!string.IsNullOrWhiteSpace(options.ApiKey))
            {
                settings = settings.Authentication(new ApiKey(options.ApiKey));
            }
            else if (!string.IsNullOrWhiteSpace(options.Username) && !string.IsNullOrWhiteSpace(options.Password))
            {
                settings = settings.Authentication(new BasicAuthentication(options.Username, options.Password));
            }

            return new ElasticsearchClient(settings);
        });
        services.AddScoped<ProductSearchService>();
        services.AddScoped<IProductSearchService>(provider => provider.GetRequiredService<ProductSearchService>());
        services.AddScoped<IProductSearchIndexService>(provider => provider.GetRequiredService<ProductSearchService>());
        services.AddScoped<IProductSearchRebuildJob, ProductSearchRebuildJob>();
    }

    private static void ConfigureRedis(this IServiceCollection services)
    {
        var settings = services.GetOptions<RedisOptions>(nameof(RedisOptions));
        if (string.IsNullOrEmpty(settings.ConnectionString))
        {
            throw new ArgumentException("Redis Conenction string is not configured!");
        }

        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = settings.ConnectionString;
        });
    }

    private static void ConfigureHangfire(this IServiceCollection services, IConfiguration configuration)
    {
        var databaseOptions = configuration.GetSection(nameof(DatabaseOptions)).Get<DatabaseOptions>();
        if (databaseOptions == null || string.IsNullOrEmpty(databaseOptions.ConnectionString))
            throw new ArgumentException("Database connection string is not configured for Hangfire.");

        services.AddHangfire(config => config
            .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
            .UseSimpleAssemblyNameTypeSerializer()
            .UseRecommendedSerializerSettings()
            .UseSqlServerStorage(databaseOptions.ConnectionString, new SqlServerStorageOptions
            {
                CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
                SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                QueuePollInterval = TimeSpan.Zero,
                UseRecommendedIsolationLevel = true,
                DisableGlobalLocks = true
            }));

        services.AddHangfireServer();
    }
}
