using FluentValidation;

namespace SchoolApp.Domain.Specifications.User.Interfaces;

public interface IUpdateUserSpecification
{
    void AddRuleTaxDocumentShouldNotEmpty(AbstractValidator<Entities.User> validator);
    void AddRulePersonTypeShouldNotEmpty(AbstractValidator<Entities.User> validator);
    void AddRuleTaxDocumentShouldBeValid(AbstractValidator<Entities.User> validator);
    void AddRuleFullNameShouldNotEmpty(AbstractValidator<Entities.User> validator);
    void AddRuleManagedSchoolIdShouldAdded(AbstractValidator<Entities.User> validator);
    void AddRuleTaxDocumentShouldBeUnique(AbstractValidator<Entities.User> validator);
}