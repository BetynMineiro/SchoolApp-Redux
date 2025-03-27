using MediatR;

namespace SchoolApp.Application.UserCases.Authentication.SignUp.Dto;

public class SignUpInput : IRequest<SignUpOutput>
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}