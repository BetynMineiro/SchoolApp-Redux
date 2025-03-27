using MediatR;

namespace SchoolApp.Application.UserCases.User.Delete.Dto;

public class DeleteUserInput: IRequest<DeleteUserOutput>
{
    public string Id { get; set; }
}