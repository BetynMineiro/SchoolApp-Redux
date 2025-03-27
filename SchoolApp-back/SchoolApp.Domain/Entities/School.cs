using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.Domain.Entities.Base;

namespace SchoolApp.Domain.Entities;

public class School :  EntityBase
{
    public string Id { get; set; }= string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string TaxDocument { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public PersonType PersonType { get; private set; } = PersonType.Company;
    public string Address { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

}