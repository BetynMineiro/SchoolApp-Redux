using FluentValidation;
using SchoolApp.Domain.Specifications.School.Interfaces;
using SchoolApp.Domain.Validators.School.Interfaces;

namespace SchoolApp.Domain.Validators.School;

public class IsValidUpdateSchoolValidator: AbstractValidator<Entities.School>,  IIsValidUpdateSchoolValidator
{
    public IsValidUpdateSchoolValidator(IUpdateSchoolSpecification specification)
    {
        specification.AddRuleFullNameShouldNotEmpty(this);
        specification.AddRuleTaxDocumentShouldNotEmpty(this);
        specification.AddRuleTaxDocumentShouldBeValid(this);
        specification.AddRuleTaxDocumentShouldBeUnique(this);
    }
    
}