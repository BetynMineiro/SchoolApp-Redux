using MediatR;

namespace SchoolApp.Application.UserCases.Authentication.DeleteUser.Dto;

public class DeleteUserAuthInput: IRequest<DeleteUserAuthOutput>
{
    public string UserAuthId { get; set; }= string.Empty;
}