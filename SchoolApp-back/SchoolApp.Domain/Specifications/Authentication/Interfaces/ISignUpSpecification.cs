using FluentValidation;
using SchoolApp.Domain.Models.Authentication.SignUp;

namespace SchoolApp.Domain.Specifications.Authentication.Interfaces;

public interface ISignUpSpecification
{
    void AddRuleEmailShouldNotEmpty(AbstractValidator<SignUpModel> validator);
    void AddRuleNameShouldNotEmpty(AbstractValidator<SignUpModel> validator);
    void AddRulePasswordShouldNotEmpty(AbstractValidator<SignUpModel> validator);
    void AddRuleEmailShouldBeValid(AbstractValidator<SignUpModel> validator);
    void AddRulePasswordShouldBeStrong(AbstractValidator<SignUpModel> validator);
    
}