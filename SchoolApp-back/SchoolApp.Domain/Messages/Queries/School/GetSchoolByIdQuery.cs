using MediatR;
using SchoolApp.Domain.Messages.Queries.School.Dto;

namespace SchoolApp.Domain.Messages.Queries.School;

public class GetSchoolByIdQuery : IRequest<SchoolQueryResult>
{
    public string Id { get; set; }
}