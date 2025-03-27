using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Application.ApplicationServices.NotificationService;

namespace SchoolApp.Application;

public static class ApplicationModule
{
    public static void ConfigureApplicationLayer(this IServiceCollection services)
    {
        //Notification
        services.AddScoped<NotificationServiceContext>();
        
    }
}