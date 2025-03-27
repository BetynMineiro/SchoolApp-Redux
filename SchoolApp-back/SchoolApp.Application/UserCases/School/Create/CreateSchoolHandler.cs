using SchoolApp.Domain.Repositories;
using SchoolApp.Domain.Validators.School.Interfaces;
using MediatR;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.Application.UserCases.School.Create.Dto;
using Serilog;

namespace SchoolApp.Application.UserCases.School.Create;

public class CreateSchoolHandler(
    ILogger logger,
    IIsValidCreateSchoolValidator createSchoolValidator,
    NotificationServiceContext notificationServiceContext,
    ISchoolRepository schoolRepository)
    : IRequestHandler<CreateSchoolInput, CreateSchoolOutput>
{
    public async Task<CreateSchoolOutput> Handle(CreateSchoolInput request, CancellationToken cancellationToken)
    {
        logger.Information("Starting to Create new School | Method: {method} | Class: {class} | SchoolName: {schoolName}", nameof(Handle), nameof(CreateSchoolHandler),
            request.FullName);

        try
        {
            var entity = new SchoolApp.Domain.Entities.School
            {
                FullName = request.FullName,
                TaxDocument = request.TaxDocument,
                Avatar = request.Avatar,
                Address = request.Address,
                PhoneNumber = request.PhoneNumber,
                Email = request.Email,
            };

            logger.Debug("School entity created | FullName: {fullName} | Email: {email}", entity.FullName, entity.Email);

            var result = await createSchoolValidator.ValidateAsync(entity, cancellationToken);
            if (!result.IsValid)
            {
                logger.Warning("Create School validation failed | Errors: {errors}", string.Join(", ", result.Errors));
                notificationServiceContext.AddNotifications(result);
                return default;
            }

            logger.Debug("Create School validation succeeded");

            var created = await schoolRepository.AddAsync(entity, cancellationToken);
            if (string.IsNullOrWhiteSpace(created))
            {
                logger.Warning("Create new School failed | Reason: Created ID is null or empty");
                notificationServiceContext.AddNotification("Create new School Fail");
                return default;
            }

            logger.Information("Create new School {created} Finished | Method: {method} | Class: {class}", created, nameof(Handle), nameof(CreateSchoolHandler));
            return new CreateSchoolOutput { Id = created };
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Exception occurred | Method: {method} | Class: {class} | Error: {error}", nameof(Handle), nameof(CreateSchoolHandler), ex.Message);
            return default;
        }
    }
}