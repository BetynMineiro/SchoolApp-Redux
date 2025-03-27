using MediatR;

namespace SchoolApp.Application.UserCases.School.Delete.Dto;

public class DeleteSchoolInput: IRequest<DeleteSchoolOutput>
{
    public string Id { get; set; }
}