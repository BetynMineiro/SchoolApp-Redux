using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Messages.Queries.School.Dto;

namespace SchoolApp.MongoDb.Adapter.Mapper;

public static class SchoolMapper
{
    public static School ToSchoolDomain(this MongoDb.Adapter.Entities.School? school)
    {
        if (school is null)
            return default!;

        return new School
        {
            Id = school.Id.ToString(),
            TaxDocument = school.TaxDocument,
            FullName = school.FullName,
            Avatar = school.Avatar,
            Address = school.Address,
            PhoneNumber = school.PhoneNumber,
            Email = school.Email,
            Status = school.Status,
            CreatedBy = school.CreatedBy,
            CreatedAt = school.CreatedAt,
            UpdatedAt = school.UpdatedAt,
            UpdatedBy = school.UpdatedBy,
        };
    }

    public static SchoolQueryResult ToSchoolQueryResult(this MongoDb.Adapter.Entities.School? school)
    {
        if (school is null)
            return default!;

        return new SchoolQueryResult
        {
            Id = school.Id.ToString(),
            TaxDocument = school.TaxDocument,
            FullName = school.FullName,
            Avatar = school.Avatar,
            Address = school.Address,
            PhoneNumber = school.PhoneNumber,
            Email = school.Email,
            Status = school.Status,
        };
    }

}