using SchoolApp.CrossCutting.Enum.Common;

namespace SchoolApp.Domain.Messages.Queries.School.Dto;

public class SchoolQueryListResult
{
    public string Id { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public Status Status { get; set; }
    public string PhoneNumber { get; set; }= string.Empty;
    public string Email { get; set; }= string.Empty;

}