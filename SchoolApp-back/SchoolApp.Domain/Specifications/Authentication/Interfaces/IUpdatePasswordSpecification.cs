using FluentValidation;
using SchoolApp.Domain.Models.Authentication.UpdatePassword;

namespace SchoolApp.Domain.Specifications.Authentication.Interfaces;

public interface IUpdatePasswordSpecification
{
    void AddRuleIdShouldNotEmpty(AbstractValidator<UpdatePasswordModel> validator);
    void AddRulePasswordShouldNotEmpty(AbstractValidator<UpdatePasswordModel> validator);
    void AddRulePasswordShouldBeStrong(AbstractValidator<UpdatePasswordModel> validator);
}