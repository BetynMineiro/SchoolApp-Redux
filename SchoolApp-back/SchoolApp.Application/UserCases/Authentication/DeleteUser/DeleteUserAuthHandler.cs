using SchoolApp.Domain.DomainServices.Interfaces.Authentication;
using MediatR;
using SchoolApp.Application.UserCases.Authentication.DeleteUser.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.Authentication.DeleteUser;

public class DeleteUserAuthHandler(
    ILogger logger,
    IAuthenticationService authenticationService)
    : IRequestHandler<DeleteUserAuthInput, DeleteUserAuthOutput>
{
    public async Task<DeleteUserAuthOutput> Handle(DeleteUserAuthInput request, CancellationToken cancellationToken)
    {
        const string MethodName = nameof(Handle);
        const string ClassName = nameof(DeleteUserAuthHandler);

        logger.Information("Starting user auth deletion process | Method: {method} | Class: {class} | UserAuthId: {userAuthId}",
            MethodName, ClassName, request.UserAuthId);

        try
        {
            logger.Debug("Calling DeleteUserAsync | Method: {method} | Class: {class} | UserAuthId: {userAuthId}",
                MethodName, ClassName, request.UserAuthId);

            await authenticationService.DeleteUserAsync(request.UserAuthId);

            logger.Information("Successfully deleted user auth | Method: {method} | Class: {class} | UserAuthId: {userAuthId}",
                MethodName, ClassName, request.UserAuthId);

            logger.Information("Finished user auth deletion process | Method: {method} | Class: {class} | UserAuthId: {userAuthId}",
                MethodName, ClassName, request.UserAuthId);

            return new DeleteUserAuthOutput();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Error deleting user auth | Method: {method} | Class: {class} | UserAuthId: {userAuthId} | Error: {error}",
                MethodName, ClassName, request.UserAuthId, ex.Message);
            return default;
        }
    }
}