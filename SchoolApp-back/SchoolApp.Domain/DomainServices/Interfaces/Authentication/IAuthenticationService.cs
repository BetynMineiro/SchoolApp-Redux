using Auth0.ManagementApi.Models;
using SchoolApp.Domain.Models.Authentication.SignUp;

namespace SchoolApp.Domain.DomainServices.Interfaces.Authentication;

public interface IAuthenticationService
{
    Task<string> SingUpAsync(SignUpModel signUpModel, CancellationToken cancellationToken);
    Task<User> GetUserInfoAsync(string userAuthId, CancellationToken cancellationToken);
    Task UpdateUserPasswordAsync(string userAuthId, string password,CancellationToken cancellationToken);
    Task DeleteUserAsync(string userAuthId);
}