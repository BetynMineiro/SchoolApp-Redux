using MediatR;

namespace SchoolApp.Application.UserCases.School.Create.Dto;

public class CreateSchoolInput : IRequest<CreateSchoolOutput>
{
    public string FullName { get; set; }  = string.Empty;
    public string TaxDocument { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

}