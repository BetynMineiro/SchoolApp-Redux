using SchoolApp.Domain.Models.Authentication.SignUp;
using FluentValidation;
using SchoolApp.Domain.Models.Authentication.UpdatePassword;
using SchoolApp.Domain.Specifications.Authentication.Interfaces;
using SchoolApp.Domain.Validators.Authentication.Interfaces;

namespace SchoolApp.Domain.Validators.Authentication;

public class IsValidUpdatePasswordValidator : AbstractValidator<UpdatePasswordModel>,  IIsValidUpdatePasswordValidator
{
    public IsValidUpdatePasswordValidator(IUpdatePasswordSpecification updatePasswordSpecification)
    {
        updatePasswordSpecification.AddRulePasswordShouldNotEmpty(this);
        updatePasswordSpecification.AddRulePasswordShouldBeStrong(this);
        updatePasswordSpecification.AddRuleIdShouldNotEmpty(this);
        
    }
}