using SchoolApp.Domain.DomainServices.Interfaces.Authentication;
using SchoolApp.Domain.Repositories;
using SchoolApp.Domain.Validators.User.Interfaces;
using MediatR;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.Application.UserCases.User.Update.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.User.Update;

public class UpdateUserHandler(
    ILogger logger,
    IIsValidUpdateUserValidator updateUserValidator,
    NotificationServiceContext notificationServiceContext,
    IAuthenticationService authenticationService,
    IUserRepository repository)
    : IRequestHandler<UpdateUserInput, UpdateUserOutput>
{
public async Task<UpdateUserOutput> Handle(UpdateUserInput request, CancellationToken cancellationToken)
{
    logger.Information("Starting to Update the UserAuth | Method: {method} | Class: {class} | UserId: {userId}", nameof(Handle), nameof(UpdateUserHandler), request.Id);

    try
    {
        var user = await repository.GetAsync(request.Id, cancellationToken);
        if (user == null)
        {
            logger.Warning("User not found | UserId: {userId}", request.Id);
            notificationServiceContext.AddNotification($"User {request.Id} not found");
            return default;
        }

        var avatar = request.Avatar;
        if (string.IsNullOrWhiteSpace(avatar))
        {
            var userAuth0 = await authenticationService.GetUserInfoAsync(user.UserAuthId, cancellationToken);
            avatar = userAuth0?.Picture;
            logger.Debug("User Auth0 picture retrieved | UserAuthId: {userAuthId} | Avatar: {avatar}", user.UserAuthId, avatar);
        }

        var entity = new SchoolApp.Domain.Entities.User
        {
            Id = request.Id,
            TaxDocument = request.TaxDocument,
            ProfileType = request.ProfileType,
            FullName = request.FullName,
            SocialName = request.SocialName,
            BirthDay = request.BirthDay,
            Avatar = avatar,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Gender = request.Gender,
            Status = request.Status,
            ManagedSchoolId = request.ManagedSchoolId,
            PersonType = request.PersonType,
        };

        logger.Debug("User entity created for update | UserId: {userId} | FullName: {fullName}", entity.Id, entity.FullName);

        var result = await updateUserValidator.ValidateAsync(entity, cancellationToken);
        if (!result.IsValid)
        {
            logger.Warning("Update User validation failed | Errors: {errors}", string.Join(", ", result.Errors));
            notificationServiceContext.AddNotifications(result);
            return default;
        }

        logger.Debug("Update User validation succeeded");

        var updated = await repository.UpdateAsync(entity.Id, entity, cancellationToken);
        if (string.IsNullOrWhiteSpace(updated))
        {
            logger.Warning("Update UserAuth failed | UserId: {userId}", request.Id);
            notificationServiceContext.AddNotification($"Update UserAuth {request.Id} Fail");
            return default;
        }

        logger.Information("Update UserAuth {userId} Finished | Method: {method} | Class: {class}", request.Id, nameof(Handle), nameof(UpdateUserHandler));
        return new UpdateUserOutput();
    }
    catch (Exception ex)
    {
        logger.Error(ex, "Exception occurred | Method: {method} | Class: {class} | UserId: {userId} | Error: {error}", nameof(Handle), nameof(UpdateUserHandler), request.Id, ex.Message);
        return default;
    }
}
}