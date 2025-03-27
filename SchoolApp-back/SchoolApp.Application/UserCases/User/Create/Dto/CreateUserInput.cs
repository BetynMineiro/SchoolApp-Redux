using SchoolApp.Domain.Enum.Management;
using MediatR;
using SchoolApp.CrossCutting.Enum.Common;

namespace SchoolApp.Application.UserCases.User.Create.Dto;

public class CreateUserInput: IRequest<CreateUserOutput>
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string SocialName { get; set; } = string.Empty;
    public string TaxDocument { get; set; } = string.Empty;
    public ProfileType ProfileType { get;  set; } = ProfileType.None;
    public PersonType PersonType { get; set; }
    public Gender Gender { get; set; }
    public DateTimeOffset BirthDay { get; set; }
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string ManagedSchoolId { get; set; } = string.Empty;
    
}