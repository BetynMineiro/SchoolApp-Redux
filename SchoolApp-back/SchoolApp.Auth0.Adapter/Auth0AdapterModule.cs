using SchoolApp.Domain.DomainServices.Interfaces.Authentication;
using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Auth0.Adapter.Services;

namespace SchoolApp.Auth0.Adapter;

public static class Auth0AdapterModule
{
    public static void ConfigureAuth0Adapter(this IServiceCollection services)
    {
        //Services
        services.AddScoped<IAuthenticationService, AuthenticationService>();

    }
}