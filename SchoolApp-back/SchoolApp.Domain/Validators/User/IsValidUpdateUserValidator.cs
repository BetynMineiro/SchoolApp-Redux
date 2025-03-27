using FluentValidation;
using SchoolApp.Domain.Specifications.User.Interfaces;
using SchoolApp.Domain.Validators.User.Interfaces;

namespace SchoolApp.Domain.Validators.User;

public class IsValidUpdateUserValidator: AbstractValidator<Entities.User>,  IIsValidUpdateUserValidator
{
    public IsValidUpdateUserValidator(IUpdateUserSpecification specification)
    {
        specification.AddRuleFullNameShouldNotEmpty(this);
        specification.AddRuleTaxDocumentShouldNotEmpty(this);
        specification.AddRuleTaxDocumentShouldBeValid(this);
        specification.AddRuleManagedSchoolIdShouldAdded(this);
        specification.AddRulePersonTypeShouldNotEmpty(this);
        specification.AddRuleTaxDocumentShouldBeUnique(this);
    }
    
}