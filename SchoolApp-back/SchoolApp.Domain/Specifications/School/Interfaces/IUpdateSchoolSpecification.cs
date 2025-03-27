using FluentValidation;

namespace SchoolApp.Domain.Specifications.School.Interfaces;

public interface IUpdateSchoolSpecification
{
    void AddRuleTaxDocumentShouldNotEmpty(AbstractValidator<Entities.School> validator);
    void AddRuleTaxDocumentShouldBeValid(AbstractValidator<Entities.School> validator);
    void AddRuleFullNameShouldNotEmpty(AbstractValidator<Entities.School> validator);
    void AddRuleTaxDocumentShouldBeUnique(AbstractValidator<Entities.School> validator);
   
}