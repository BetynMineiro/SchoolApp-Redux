using SchoolApp.CrossCutting.RequestObjects;
using SchoolApp.CrossCutting.ResultObjects;
using MediatR;
using SchoolApp.Domain.Messages.Queries.School.Dto;

namespace SchoolApp.Domain.Messages.Queries.School;

public class GetFilteredSchoolQuery(string filterValue, PagedRequest pagedRequest)
    : IRequest<Pagination<SchoolQueryListResult>>
{
    public string? FilterValue { get; set; } = filterValue;
    public PagedRequest PagedRequest { get; set; } = pagedRequest;
}