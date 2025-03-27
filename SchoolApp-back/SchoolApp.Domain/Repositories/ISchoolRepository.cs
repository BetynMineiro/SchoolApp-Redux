using SchoolApp.CrossCutting.ResultObjects;
using MediatR;
using SchoolApp.Domain.Entities;
using SchoolApp.Domain.Messages.Queries.School;
using SchoolApp.Domain.Messages.Queries.School.Dto;

namespace SchoolApp.Domain.Repositories;

public interface ISchoolRepository :
    IRequestHandler<GetFilteredSchoolQuery, Pagination<SchoolQueryListResult>>,
    IRequestHandler<GetSchoolByIdQuery, SchoolQueryResult>
{
    Task<School?> GetAsync(string id,  CancellationToken cancellationToken);
    Task<string> AddAsync(School school, CancellationToken cancellationToken);
    Task<string> RemoveAsync(string id, CancellationToken cancellationToken);
    Task<string> UpdateAsync(string id, School school, CancellationToken cancellationToken);
    Task<School?> GetByTaxDocumentAsync(string taxDocument,  CancellationToken cancellationToken);
    
}