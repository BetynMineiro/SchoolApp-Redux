using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.CrossCutting.Configurations;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;

namespace SchoolApp.CrossCutting;

public static class CrossCuttingModule
{
    public static AppSettingsConfig GetAppSettingsApiConfig(this IConfiguration configuration)
    {
        var settingsSection = configuration.GetSection("SchoolApp");
        var settingsApi = settingsSection.Get<AppSettingsConfig>();
        return settingsApi;
    }
    
    public static void ConfigureLogging(this IServiceCollection services)
    {
        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithProcessId()
            .Enrich.WithThreadId()
            .Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().GetName().Name)
            .Enrich.WithExceptionDetails()
            .MinimumLevel.Is(LogEventLevel.Information)
            .WriteTo.Console()
            .CreateLogger();

        services.AddSingleton(Log.Logger);
    }
    
    public static void ConfigureMessageHandler(this IServiceCollection services, List<Assembly> assemblyCollection)
    {
        services.AddMediatR(config => 
        {
            config.RegisterServicesFromAssemblies(assemblyCollection.ToArray());
        });
    }
}