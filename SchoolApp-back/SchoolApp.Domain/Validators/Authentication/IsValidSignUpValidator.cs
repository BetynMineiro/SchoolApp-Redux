using FluentValidation;
using SchoolApp.Domain.Models.Authentication.SignUp;
using SchoolApp.Domain.Specifications.Authentication.Interfaces;
using SchoolApp.Domain.Validators.Authentication.Interfaces;

namespace SchoolApp.Domain.Validators.Authentication;

public class IsValidSignUpValidator : AbstractValidator<SignUpModel>,  IIsValidSignUpValidator
{
    public IsValidSignUpValidator(ISignUpSpecification signUpSpecification)
    {
        signUpSpecification.AddRuleEmailShouldNotEmpty(this);
        signUpSpecification.AddRuleEmailShouldBeValid(this);
        signUpSpecification.AddRulePasswordShouldNotEmpty(this);
        signUpSpecification.AddRuleNameShouldNotEmpty(this);
        signUpSpecification.AddRulePasswordShouldBeStrong(this);
    }
}