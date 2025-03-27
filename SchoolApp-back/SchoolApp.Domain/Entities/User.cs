using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.Domain.Entities.Base;
using SchoolApp.Domain.Enum.Management;

namespace SchoolApp.Domain.Entities;

public class User :  EntityBase
{
    public string Id { get; set; }= string.Empty;
    public string? UserAuthId { get; set; }= string.Empty;
    public ProfileType ProfileType { get;  set; } = ProfileType.None;
    public string? FullName { get; set; } = string.Empty;
    public string? SocialName { get; set; } = string.Empty;
    public string? TaxDocument { get; set; } = string.Empty;
    public string? Avatar { get; set; } = string.Empty;
    public PersonType PersonType { get;  set; } = PersonType.Person;
    public Gender Gender { get; set; }
    public DateTimeOffset BirthDay { get; set; }
    public  string? Address { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? ManagedSchoolId { get; set; }= string.Empty;

}