using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.Domain.Enum.Management;

namespace SchoolApp.Domain.Messages.Queries.User.Dto;

public class UserByAuthIdQueryResult
{
    public string Id { get; set; }
    public ProfileType ProfileType { get; set; }
    public string? FullName { get; set; }
    public string? Avatar { get; set; }
    public string? Email { get; set; }
    public string? ManagedSchoolId { get; set; }= string.Empty;
    public bool IsActive { get; set; }
}