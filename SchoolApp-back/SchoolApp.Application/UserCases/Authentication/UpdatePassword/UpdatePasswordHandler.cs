using SchoolApp.Domain.DomainServices.Interfaces.Authentication;
using SchoolApp.Domain.Models.Authentication.UpdatePassword;
using SchoolApp.Domain.Repositories;
using SchoolApp.Domain.Validators.Authentication.Interfaces;
using MediatR;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.Application.UserCases.Authentication.UpdatePassword.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.Authentication.UpdatePassword;

public class UpdatePasswordHandler(
    ILogger logger,
    IIsValidUpdatePasswordValidator isValidUpdatePasswordValidator,
    IAuthenticationService authenticationService,
    NotificationServiceContext notificationServiceContext,
    IUserRepository repository)
    : IRequestHandler<UpdatePasswordInput, UpdatePasswordOutput>
{
    public async Task<UpdatePasswordOutput> Handle(UpdatePasswordInput request, CancellationToken cancellationToken)
    {
        logger.Information("Starting to Update Password | Method: {method} | Class: {class} | UserId: {userId}", nameof(Handle), nameof(UpdatePasswordHandler), request.Id);

        try
        {
            var passwordModel = new UpdatePasswordModel { Id = request.Id, Password = request.Password };
            logger.Debug("UpdatePasswordModel created | UserId: {userId}", passwordModel.Id);

            var result = await isValidUpdatePasswordValidator.ValidateAsync(passwordModel, cancellationToken);
            if (!result.IsValid)
            {
                logger.Warning("Update Password validation failed | Errors: {errors}", string.Join(", ", result.Errors));
                notificationServiceContext.AddNotifications(result);
                return default;
            }

            logger.Debug("Update Password validation succeeded");

            var user = await repository.GetAsync(request.Id, cancellationToken);
            if (user == null)
            {
                logger.Warning("User not found | UserId: {userId}", request.Id);
                notificationServiceContext.AddNotification($"User {request.Id} not found");
                return default;
            }

            logger.Debug("User retrieved | UserId: {userId} | UserAuthId: {userAuthId}", user.Id, user.UserAuthId);

            await authenticationService.UpdateUserPasswordAsync(user.UserAuthId, passwordModel.Password, cancellationToken);
            logger.Information("Update Password Finished | Method: {method} | Class: {class} | UserId: {userId}", nameof(Handle), nameof(UpdatePasswordHandler), request.Id);

            return new UpdatePasswordOutput();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Exception occurred | Method: {method} | Class: {class} | Error: {error}", nameof(Handle), nameof(UpdatePasswordHandler), ex.Message);
            return default;
        }
    }
}