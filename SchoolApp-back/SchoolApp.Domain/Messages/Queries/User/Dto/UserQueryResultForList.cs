using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.Domain.Enum.Management;

namespace SchoolApp.Domain.Messages.Queries.User.Dto;

public class UserQueryResultForList
{
    public string Id { get; set; }= string.Empty;
    public string? FullName { get; set; } = string.Empty;
    public string? SocialName { get; set; } = string.Empty;
    public string? TaxDocument { get; set; } = string.Empty;
    public string? Avatar { get; set; } = string.Empty;
    public ProfileType ProfileType { get;  set; } = ProfileType.None;
    public Status Status { get; set; }
    public string? PhoneNumber { get; set; }= string.Empty;
    public string? Email { get; set; }= string.Empty;

}