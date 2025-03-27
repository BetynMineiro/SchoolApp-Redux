using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.Domain.Enum.Management;

namespace SchoolApp.Domain.Messages.Queries.User.Dto;

public class UserQueryResult
{
    public string Id { get; set; } = string.Empty;
    public string? FullName { get; set; } = string.Empty;
    public string? SocialName { get; set; } = string.Empty;
    public string? TaxDocument { get; set; } = string.Empty;
    public ProfileType ProfileType { get; set; } = ProfileType.None;
    public string? Avatar { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Gender Gender { get; set; }
    public DateTimeOffset BirthDay { get; set; }
    public string? Address { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; } = string.Empty;
    public string? Email { get; set; } = string.Empty;
    public string? ManagedSchoolId { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public DateTimeOffset CreatedAt { get; set; }
    public string UpdatedBy { get; set; } = string.Empty;
    public DateTimeOffset? UpdatedAt { get; set; }
    public PersonType PersonType { get; set; }
}