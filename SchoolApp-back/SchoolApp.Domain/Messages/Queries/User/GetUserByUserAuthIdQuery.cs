using MediatR;
using SchoolApp.Domain.Messages.Queries.User.Dto;

namespace SchoolApp.Domain.Messages.Queries.User;

public class GetUserByUserAuthIdQuery: IRequest<UserByAuthIdQueryResult>
{

}