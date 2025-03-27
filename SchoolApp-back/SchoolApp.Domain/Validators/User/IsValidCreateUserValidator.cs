using FluentValidation;
using SchoolApp.Domain.Specifications.User.Interfaces;
using SchoolApp.Domain.Validators.User.Interfaces;

namespace SchoolApp.Domain.Validators.User;

public class IsValidCreateUserValidator: AbstractValidator<Entities.User>,  IIsValidCreateUserValidator
{
    public IsValidCreateUserValidator(ICreateUserSpecification specification)
    {
        specification.AddRuleFullNameShouldNotEmpty(this);
        specification.AddRuleTaxDocumentShouldNotEmpty(this);
        specification.AddRuleTaxDocumentShouldBeValid(this);
        specification.AddRuleTaxDocumentShouldBeUnique(this);
        specification.AddRuleManagedSchoolIdShouldAdded(this);
        specification.AddRulePersonTypeShouldNotEmpty(this);
    }
    
}