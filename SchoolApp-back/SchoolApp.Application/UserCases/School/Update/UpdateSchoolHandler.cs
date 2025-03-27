using SchoolApp.Domain.Repositories;
using SchoolApp.Domain.Validators.School.Interfaces;
using MediatR;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.Application.UserCases.School.Update.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.School.Update;

public class UpdateSchoolHandler(
    ILogger logger,
    IIsValidUpdateSchoolValidator updateSchoolValidator,
    NotificationServiceContext notificationServiceContext,
    ISchoolRepository schoolRepository)
    : IRequestHandler<UpdateSchoolInput, UpdateSchoolOutput>

{
    public async Task<UpdateSchoolOutput> Handle(UpdateSchoolInput request, CancellationToken cancellationToken)
    {
        logger.Information("Starting to Update the School | Method: {method} | Class: {class} | SchoolId: {schoolId}", nameof(Handle), nameof(UpdateSchoolHandler), request.Id);

        try
        {
            var entity = new SchoolApp.Domain.Entities.School
            {
                Id = request.Id,
                FullName = request.FullName,
                TaxDocument = request.TaxDocument,
                Avatar = request.Avatar,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
                Status = request.Status,

            };

            logger.Debug("School entity created for update | SchoolId: {schoolId} | FullName: {fullName}", entity.Id, entity.FullName);

            var result = await updateSchoolValidator.ValidateAsync(entity, cancellationToken);
            if (!result.IsValid)
            {
                logger.Warning("Update School validation failed | Errors: {errors}", string.Join(", ", result.Errors));
                notificationServiceContext.AddNotifications(result);
                return default;
            }

            logger.Debug("Update School validation succeeded");

            var updated = await schoolRepository.UpdateAsync(entity.Id, entity, cancellationToken);
            if (string.IsNullOrWhiteSpace(updated))
            {
                logger.Warning("Update School failed | SchoolId: {schoolId}", request.Id);
                notificationServiceContext.AddNotification($"Update School {request.Id} Fail");
                return default;
            }

            logger.Information("Update School {schoolId} Finished | Method: {method} | Class: {class}", request.Id, nameof(Handle), nameof(UpdateSchoolHandler));
            return new UpdateSchoolOutput();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Exception occurred | Method: {method} | Class: {class} | SchoolId: {schoolId} | Error: {error}", nameof(Handle), nameof(UpdateSchoolHandler),
                request.Id, ex.Message);
            return default;
        }
    }

}