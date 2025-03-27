using SchoolApp.CrossCutting.Enum.Common;
using MongoDB.Bson.Serialization.Attributes;
using SchoolApp.MongoDb.Adapter.Entities.Base;

namespace SchoolApp.MongoDb.Adapter.Entities;

public class School : MongoBaseEntity
{
    [BsonElement] public string? FullName { get; set; } = string.Empty;
    [BsonElement] public string? TaxDocument { get; set; } = string.Empty;
    [BsonElement] public string? Avatar { get; set; } = string.Empty;
    [BsonElement] public PersonType PersonType { get; private set; } = PersonType.Company;
    [BsonElement] public string? Address { get; set; } = string.Empty;
    [BsonElement] public string? PhoneNumber { get; set; } = string.Empty;
    [BsonElement] public string? Email { get; set; } = string.Empty;
   
}