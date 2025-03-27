using SchoolApp.CrossCutting.ResultObjects;
using MediatR;
using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Messages.Queries.User;
using SchoolApp.Domain.Messages.Queries.User.Dto;

namespace SchoolApp.Domain.Repositories;

public interface IUserRepository:
    IRequestHandler<GetFilteredUsersQuery, Pagination<UserQueryResultForList>>,
    IRequestHandler<GetUserByIdQuery, UserQueryResult>,
    IRequestHandler<GetUserByUserAuthIdQuery, UserByAuthIdQueryResult>
    
{
    Task<User?> GetAsync(string id,  CancellationToken cancellationToken);
    Task<IEnumerable<User?>> GetAsync(IEnumerable<string> ids,  CancellationToken cancellationToken);

    Task<string> AddAsync(User user, CancellationToken cancellationToken);
    Task<string> RemoveAsync(string id, CancellationToken cancellationToken);
    Task<string> UpdateAsync(string id, User user, CancellationToken cancellationToken);
    Task<User?> GetByTaxDocumentAsync(string? taxDocument,  CancellationToken cancellationToken);
}