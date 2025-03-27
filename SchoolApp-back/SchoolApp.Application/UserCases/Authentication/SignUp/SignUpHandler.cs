using SchoolApp.Domain.DomainServices.Interfaces.Authentication;
using SchoolApp.Domain.Models.Authentication.SignUp;
using SchoolApp.Domain.Validators.Authentication.Interfaces;
using MediatR;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.Application.UserCases.Authentication.SignUp.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.Authentication.SignUp;

public class SignUpHandler(
    ILogger logger,
    IIsValidSignUpValidator isValidSignUpValidator,
    IAuthenticationService authenticationService,
    NotificationServiceContext notificationServiceContext)
    : IRequestHandler<SignUpInput, SignUpOutput>
{
    public async Task<SignUpOutput> Handle(SignUpInput request, CancellationToken cancellationToken)
    {
        logger.Information("Starting to Signup | Method: {method} | Class: {class} | Email: {email}", nameof(Handle), nameof(SignUpHandler), request.Email);

        try
        {
            var loginEmail = new SignUpModel { Email = request.Email, Password = request.Password, Name = request.Name };
            logger.Debug("SignUpModel created | Email: {email} | Name: {name}", loginEmail.Email, loginEmail.Name);

            var result = await isValidSignUpValidator.ValidateAsync(loginEmail, cancellationToken);
            if (!result.IsValid)
            {
                logger.Warning("SignUp validation failed | Errors: {errors}", string.Join(", ", result.Errors));
                notificationServiceContext.AddNotifications(result);
                return default;
            }

            logger.Debug("SignUp validation succeeded");

            var created = await authenticationService.SingUpAsync(loginEmail, cancellationToken);
            if (string.IsNullOrEmpty(created))
            {
                logger.Warning("Signup failed | Reason: Created ID is null or empty");
                notificationServiceContext.AddNotification("Signup Fail");
                return default;
            }

            logger.Debug("User signed up | CreatedId: {createdId}", created);

            var userAuth0 = await authenticationService.GetUserInfoAsync(created, cancellationToken);
            logger.Information("Signup Finished | Method: {method} | Class: {class}", nameof(Handle), nameof(SignUpHandler));

            return new SignUpOutput { CreatedId = created, Avatar = userAuth0.Picture };
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Exception occurred | Method: {method} | Class: {class} | Error: {error}", nameof(Handle), nameof(SignUpHandler), ex.Message);
            return default;
        }
    }
}