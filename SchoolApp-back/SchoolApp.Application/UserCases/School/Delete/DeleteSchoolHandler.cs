using SchoolApp.Domain.Repositories;
using MediatR;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.Application.UserCases.School.Delete.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.School.Delete;

public class DeleteSchoolHandler(
    ILogger logger,
    NotificationServiceContext notificationServiceContext,
    ISchoolRepository schoolRepository)
    : IRequestHandler<DeleteSchoolInput, DeleteSchoolOutput>
{
    public async Task<DeleteSchoolOutput> Handle(DeleteSchoolInput request, CancellationToken cancellationToken)
    {
        logger.Information("Starting to Delete School | Method: {method} | Class: {class} | SchoolId: {schoolId}", nameof(Handle), nameof(DeleteSchoolHandler), request.Id);

        try
        {
            var deleted = await schoolRepository.RemoveAsync(request.Id, cancellationToken);
            if (string.IsNullOrWhiteSpace(deleted))
            {
                logger.Warning("Delete School failed | SchoolId: {schoolId}", request.Id);
                notificationServiceContext.AddNotification($"Delete School {request.Id} Fail");
                return default;
            }

            logger.Information("Delete School {schoolId} Finished | Method: {method} | Class: {class}", request.Id, nameof(Handle), nameof(DeleteSchoolHandler));
            return new DeleteSchoolOutput();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Exception occurred | Method: {method} | Class: {class} | SchoolId: {schoolId} | Error: {error}", nameof(Handle), nameof(DeleteSchoolHandler), request.Id, ex.Message);
            return default;
        }
    }
}