using SchoolApp.CrossCutting.RequestObjects;
using SchoolApp.CrossCutting.ResultObjects;
using MediatR;
using SchoolApp.Domain.Enum.Management;
using SchoolApp.Domain.Messages.Queries.User.Dto;

namespace SchoolApp.Domain.Messages.Queries.User;

public class GetFilteredUsersQuery : IRequest<Pagination<UserQueryResultForList>>
{
    public GetFilteredUsersQuery(string? filter,  ProfileType profileType, PagedRequest pagedRequest)
    {
        FilterValue = filter;
        ProfileType = profileType;
        PagedRequest = pagedRequest;
    }

    public string? FilterValue { get; set; }  
    public ProfileType ProfileType { get; set; }  
    public PagedRequest PagedRequest { get; set; }
}