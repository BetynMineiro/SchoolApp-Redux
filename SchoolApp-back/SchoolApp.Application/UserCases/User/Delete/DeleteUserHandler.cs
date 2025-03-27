using SchoolApp.CrossCutting.Bus;
using SchoolApp.Domain.Repositories;
using MediatR;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.Application.UserCases.Authentication.DeleteUser.Dto;
using SchoolApp.Application.UserCases.User.Delete.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.User.Delete;

public class DeleteUserHandler(
    ILogger logger,
    IBus bus,
    NotificationServiceContext notificationServiceContext,
    IUserRepository repository)
    : IRequestHandler<DeleteUserInput, DeleteUserOutput>
{
    public async Task<DeleteUserOutput> Handle(DeleteUserInput request, CancellationToken cancellationToken)
    {
        logger.Information("Starting to Delete UserAuth | Method: {method} | Class: {class} | UserId: {userId}", nameof(Handle), nameof(DeleteUserHandler), request.Id);

        try
        {
            var user = await repository.GetAsync(request.Id, cancellationToken);
            if (user == null)
            {
                logger.Warning("User not found | UserId: {userId}", request.Id);
                notificationServiceContext.AddNotification($"User {request.Id} not found");
                return default;
            }

            logger.Debug("User retrieved for deletion | UserId: {userId} | UserAuthId: {userAuthId}", user.Id, user.UserAuthId);

            if (await bus.SendCommandAsync(new DeleteUserAuthInput { UserAuthId = user.UserAuthId }, cancellationToken) is not DeleteUserAuthOutput)
            {
                logger.Warning("Delete UserAuth command failed | UserAuthId: {userAuthId}", user.UserAuthId);
                return default;
            }

            var deleted = await repository.RemoveAsync(request.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(deleted))
            {
                logger.Warning("Delete UserAuth failed | UserId: {userId}", request.Id);
                notificationServiceContext.AddNotification($"Delete UserAuth {request.Id} Fail");
                return default;
            }

            logger.Information("Delete UserAuth {userId} Finished | Method: {method} | Class: {class}", request.Id, nameof(Handle), nameof(DeleteUserHandler));
            return new DeleteUserOutput();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Exception occurred | Method: {method} | Class: {class} | UserId: {userId} | Error: {error}", nameof(Handle), nameof(DeleteUserHandler), request.Id, ex.Message);
            return default;
        }
    }
}