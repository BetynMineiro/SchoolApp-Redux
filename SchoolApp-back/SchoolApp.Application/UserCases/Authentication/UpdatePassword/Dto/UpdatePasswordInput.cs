using MediatR;

namespace SchoolApp.Application.UserCases.Authentication.UpdatePassword.Dto;

public class UpdatePasswordInput: IRequest<UpdatePasswordOutput>
{
    public string Id { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}