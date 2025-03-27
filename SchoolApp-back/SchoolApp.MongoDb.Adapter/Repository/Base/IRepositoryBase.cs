using System.Linq.Expressions;
using SchoolApp.CrossCutting.ResultObjects;
using MongoDB.Driver;
using SchoolApp.MongoDb.Adapter.Entities.Base;

namespace SchoolApp.MongoDb.Adapter.Repository.Base;

public interface IRepositoryBase<T> where T : MongoBaseEntity
{
    Task<string> InsertAsync(T entity, CancellationToken cancellationToken);
    Task<T> GetByIdAsync(string id, CancellationToken cancellationToken);
    Task<T> GetSingleAsync(FilterDefinition<T> filter, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<Pagination<T>> GetAllPagedAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
    Task<IEnumerable<T>> GetByFilterAsync(FilterDefinition<T> filter, CancellationToken cancellationToken);
    Task<Pagination<T>> GetByFilterPagedAsync(FilterDefinition<T> filter, int pageNumber, int pageSize, CancellationToken cancellationToken);

    Task<Pagination<TProjection>> GetByFilterPagedAsync<TProjection>(FilterDefinition<T> filter, int pageNumber, int pageSize,
        Expression<Func<T, TProjection>> projection, CancellationToken cancellationToken);

    Task UpdateAsync(string id, T entity, CancellationToken cancellationToken);
    Task DeleteAsync(string id, CancellationToken cancellationToken);
}