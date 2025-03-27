using SchoolApp.CrossCutting.Bus;
using SchoolApp.Domain.Repositories;
using SchoolApp.Domain.Validators.User.Interfaces;
using MediatR;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.Application.UserCases.Authentication.DeleteUser.Dto;
using SchoolApp.Application.UserCases.Authentication.SignUp.Dto;
using SchoolApp.Application.UserCases.User.Create.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.User.Create;

public class CreateUserHandler(
    ILogger logger,
    IBus bus,
    IIsValidCreateUserValidator createUserValidator,
    NotificationServiceContext notificationServiceContext,
    IUserRepository repository)
    : IRequestHandler<CreateUserInput, CreateUserOutput>
{
 public async Task<CreateUserOutput> Handle(CreateUserInput request, CancellationToken cancellationToken)
{
    logger.Information("Starting to Create new UserAuth | Method: {method} | Class: {class} | UserEmail: {email}", nameof(Handle), nameof(CreateUserHandler), request.Email);

    try
    {
        var entity = new SchoolApp.Domain.Entities.User
        {
            TaxDocument = request.TaxDocument,
            ProfileType = request.ProfileType,
            FullName = request.FullName,
            SocialName = request.SocialName,
            BirthDay = request.BirthDay,
            Address = request.Address,
            PhoneNumber = request.PhoneNumber,
            Email = request.Email,
            Gender = request.Gender,
            PersonType = request.PersonType,
            ManagedSchoolId = request.ManagedSchoolId,
        };

        logger.Debug("User entity created | FullName: {fullName} | Email: {email}", entity.FullName, entity.Email);

        var signUpInputResult = await bus.SendCommandAsync(new SignUpInput
        {
            Name = request.FullName,
            Email = request.Email,
            Password = request.Password
        }, cancellationToken) as SignUpOutput;

        if (signUpInputResult == null)
        {
            logger.Warning("SignUp failed for UserAuth | Email: {email}", request.Email);
            return default;
        }

        entity.UserAuthId = signUpInputResult.CreatedId;
        entity.Avatar = signUpInputResult.Avatar;

        logger.Debug("SignUp succeeded | UserAuthId: {userAuthId} | Avatar: {avatar}", entity.UserAuthId, entity.Avatar);

        var result = await createUserValidator.ValidateAsync(entity, cancellationToken);
        if (!result.IsValid)
        {
            logger.Warning("Create User validation failed | Errors: {errors}", string.Join(", ", result.Errors));
            await bus.SendCommandAsync(new DeleteUserAuthInput { UserAuthId = entity.UserAuthId }, cancellationToken);
            notificationServiceContext.AddNotifications(result);
            return default;
        }

        logger.Debug("Create User validation succeeded");

        var created = await repository.AddAsync(entity, cancellationToken);
        if (string.IsNullOrWhiteSpace(created))
        {
            logger.Warning("Create new UserAuth failed | UserAuthId: {userAuthId}", entity.UserAuthId);
            notificationServiceContext.AddNotification("Create new UserAuth Fail");
            return default;
        }

        logger.Information("Create new UserAuth {userAuthId} Finished | Method: {method} | Class: {class}", created, nameof(Handle), nameof(CreateUserHandler));
        return new CreateUserOutput { Id = created };
    }
    catch (Exception ex)
    {
        logger.Error(ex, "Exception occurred | Method: {method} | Class: {class} | UserEmail: {email} | Error: {error}", nameof(Handle), nameof(CreateUserHandler), request.Email, ex.Message);
        return default;
    }
}
}