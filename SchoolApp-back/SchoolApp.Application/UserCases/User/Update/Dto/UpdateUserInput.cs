using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.Domain.Enum.Management;
using MediatR;

namespace SchoolApp.Application.UserCases.User.Update.Dto;

public class UpdateUserInput: IRequest<UpdateUserOutput>
{  
    public string Id { get; set; }= string.Empty;
    public ProfileType ProfileType { get;  set; } = ProfileType.None;
    public PersonType PersonType { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string SocialName { get; set; } = string.Empty;
    public string TaxDocument { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public Status Status { get; set; }
    public Gender Gender { get; set; }
    public DateTimeOffset BirthDay { get; set; }
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ManagedSchoolId { get; set; } = string.Empty;
}