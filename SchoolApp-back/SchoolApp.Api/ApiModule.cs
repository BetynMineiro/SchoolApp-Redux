using System.Reflection;
using SchoolApp.Application;
using SchoolApp.Auth0.Adapter;
using SchoolApp.CrossCutting;
using SchoolApp.CrossCutting.Bus;
using SchoolApp.CrossCutting.Configurations;
using SchoolApp.Domain;
using SchoolApp.MongoDb.Adapter;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SchoolApp.Api.Middleware;

namespace SchoolApp.Api;

public static class ApiModule
{
    public static void ConfigureFilters(this MvcOptions options)
    {
        options.Filters.Add<NotificationServiceMiddleware>();
        options.Filters.Add<AcceptHeaderMiddleware>();
        options.Filters.Add(new ProducesAttribute("application/json"));
    }

    private static void ConfigureAppSettingsApi(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.Configure<AppSettingsConfig>(configuration.GetSection("SchoolApp"));
        services.AddSingleton(resolver => resolver.GetRequiredService<IOptions<AppSettingsConfig>>().Value);
    }

    private static void ConfigureMessageHandlerLayer(this IServiceCollection services)
    {
        var assemblyCollection = new List<Assembly>
        {
            typeof(ApplicationModule).Assembly,
            typeof(MongoDbAdapterModule).Assembly,
        };
        services.ConfigureMessageHandler(assemblyCollection);
    }

    public static void ConfigureApiServicesLayer(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddScoped<IBus, Bus>();
        services.ConfigureAppSettingsApi(configuration);
        //Authetication Service
        services.ConfigureAuth0Adapter();
        //MongoDB Service
        services.ConfigureMongoAdapterLayer(configuration);
        
        services.ConfigureDomainLayer();
        services.ConfigureApplicationLayer();
        
        services.ConfigureLogging();
        services.ConfigureMessageHandlerLayer();
    }
}