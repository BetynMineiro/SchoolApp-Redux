using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.Domain.Enum;
using SchoolApp.Domain.Enum.Management;
using SchoolApp.Domain.Messages.Queries.User.Dto;
using MongoDB.Bson.Serialization.Attributes;
using SchoolApp.MongoDb.Adapter.Entities.Base;

namespace SchoolApp.MongoDb.Adapter.Entities;

public class User : MongoBaseEntity
{
    [BsonElement] public string? UserAuthId { get; set; }
    [BsonElement] public ProfileType ProfileType { get;  set; } = ProfileType.None;
    [BsonElement] public string? FullName { get; set; } 
    [BsonElement] public string? SocialName { get; set; } 
    [BsonElement] public string? TaxDocument { get; set; } 
    [BsonElement] public string? Avatar { get; set; } 
    [BsonElement] public PersonType PersonType { get; set; }
    [BsonElement] public Gender Gender { get; set; }
    [BsonElement] public DateTimeOffset BirthDay { get; set; }
    [BsonElement] public string? Address { get; set; }
    [BsonElement] public string? PhoneNumber { get; set; }
    [BsonElement] public string? Email { get; set; }
    [BsonElement] public string? ManagedSchoolId { get; set; }


}