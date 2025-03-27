using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Messages.Queries.User.Dto;

namespace SchoolApp.MongoDb.Adapter.Mapper;

public static class UserMapper
{
    public static User ToUserDomain(this MongoDb.Adapter.Entities.User? user)
    {
        if (user is null)
            return default!;

        return new User
        {
            Id = user.Id.ToString(),
            UserAuthId = user.UserAuthId,
            TaxDocument = user.TaxDocument,
            FullName = user.FullName,
            SocialName = user.SocialName,
            BirthDay = user.BirthDay,
            Avatar = user.Avatar,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Gender = user.Gender,
            Status = user.Status,
            ManagedSchoolId = user.ManagedSchoolId,
            CreatedBy = user.CreatedBy,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            UpdatedBy = user.UpdatedBy,

        };
    }

    public static UserByAuthIdQueryResult ToUserByAuthIdQueryResult(this MongoDb.Adapter.Entities.User? user)
    {
        if (user is null)
            return default!;
        return new UserByAuthIdQueryResult
        {   Id = user.Id.ToString(),
            ProfileType = user.ProfileType,
            FullName = user.FullName,
            Avatar = user.Avatar,
            Email = user.Email,
            ManagedSchoolId = user.ManagedSchoolId,
            IsActive = user.Status == Status.Active
        };
    }
    
    public static UserQueryResult ToUserQueryResult(this MongoDb.Adapter.Entities.User? user)
    {
        if (user is null)
            return default!;

        return new UserQueryResult
        {
            Id = user.Id.ToString(),
            TaxDocument = user.TaxDocument,
            ProfileType = user.ProfileType,
            PersonType = user.PersonType,
            FullName = user.FullName,
            SocialName = user.SocialName,
            BirthDay = user.BirthDay,
            Avatar = user.Avatar,
            Address = user.Address,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Gender = user.Gender,
            Status = user.Status,
            ManagedSchoolId = user.ManagedSchoolId,
            CreatedBy = user.CreatedBy,
            CreatedAt = user.CreatedAt,
            UpdatedAt = user.UpdatedAt,
            UpdatedBy = user.UpdatedBy,
      
        };
    }

    public static IEnumerable<UserQueryResultForList> ToUserQueryResultList(
        this IEnumerable<MongoDb.Adapter.Entities.User> users)
    {
        if (!users.Any())
        {
            return default;
        }

        return users.Select(user => new UserQueryResultForList()
        {
            Id = user.Id.ToString(),
            TaxDocument = user.TaxDocument,
            FullName = user.FullName,
            SocialName = user.SocialName,
            ProfileType = user.ProfileType,
            Avatar = user.Avatar,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Status = user.Status
        });
    }
}