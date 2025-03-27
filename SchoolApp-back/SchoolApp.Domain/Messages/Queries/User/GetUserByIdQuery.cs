using MediatR;
using SchoolApp.Domain.Messages.Queries.User.Dto;

namespace SchoolApp.Domain.Messages.Queries.User;

public class GetUserByIdQuery : IRequest<UserQueryResult>
{
    public string Id { get; set; }
}