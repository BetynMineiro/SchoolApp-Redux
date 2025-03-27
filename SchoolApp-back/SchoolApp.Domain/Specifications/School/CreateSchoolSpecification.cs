using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.CrossCutting.Extensions;
using FluentValidation;
using SchoolApp.Domain.Repositories;
using SchoolApp.Domain.Specifications.School.Interfaces;

namespace SchoolApp.Domain.Specifications.School;

public class CreateSchoolSpecification(ISchoolRepository repository) : ICreateSchoolSpecification
{
    private const string TAXDOCUMENT_IS_REQUIRED_MESSAGE = "Tax document is required";
    private const string FULLNAME_IS_REQUIRED_MESSAGE = "Full Name is required";
    private const string TAXDOCUMENT_IS_VALID_MESSAGE = "Tax document needs to be valid";
    private const string TAXDOCUMENT_IS_UNIQUE_MESSAGE = "Tax document needs to be unique";

    public void AddRuleTaxDocumentShouldNotEmpty(AbstractValidator<Entities.School> validator)
    {
        validator.RuleFor(entity => entity.TaxDocument)
            .NotEmpty().WithMessage(TAXDOCUMENT_IS_REQUIRED_MESSAGE);
    }


    public void AddRuleTaxDocumentShouldBeValid(AbstractValidator<Entities.School> validator)
    {
        validator.When(
            x => x.PersonType == PersonType.Company,
            () =>
            {
                validator.RuleFor(entity => entity.TaxDocument).Must((taxDocument) => taxDocument.CompanyTaxDocumentValidation())
                    .WithMessage(TAXDOCUMENT_IS_VALID_MESSAGE);
            });
        
        validator.When(
            x => x.PersonType == PersonType.Person,
            () =>
            {
                validator.RuleFor(entity => entity.TaxDocument).Must((taxDocument) => taxDocument.PersonTaxDocumentValidation())
                    .WithMessage(TAXDOCUMENT_IS_VALID_MESSAGE);
            });
 
    }

    public void AddRuleFullNameShouldNotEmpty(AbstractValidator<Entities.School> validator)
    {
        validator.RuleFor(entity => entity.FullName)
            .NotEmpty().WithMessage(FULLNAME_IS_REQUIRED_MESSAGE);
    }

    public void AddRuleTaxDocumentShouldBeUnique(AbstractValidator<Entities.School> validator)
    {
        validator.RuleFor(entity => entity.TaxDocument)
            .MustAsync(async (taxDocument, cancellationToken) =>
            {
                var school = await repository.GetByTaxDocumentAsync(taxDocument, cancellationToken: cancellationToken);
                return school == null;
            })
            .WithMessage(TAXDOCUMENT_IS_UNIQUE_MESSAGE);
    }
}