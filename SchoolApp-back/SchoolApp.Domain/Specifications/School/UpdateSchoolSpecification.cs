using SchoolApp.CrossCutting.Extensions;
using FluentValidation;
using SchoolApp.Domain.Repositories;
using SchoolApp.Domain.Specifications.School.Interfaces;

namespace SchoolApp.Domain.Specifications.School;

public class UpdateSchoolSpecification(ISchoolRepository repository) : IUpdateSchoolSpecification
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
        validator.RuleFor(entity => entity.TaxDocument).Must((c) => c.CompanyTaxDocumentValidation())
            .WithMessage(TAXDOCUMENT_IS_VALID_MESSAGE);
    }

    public void AddRuleFullNameShouldNotEmpty(AbstractValidator<Entities.School> validator)
    {
        validator.RuleFor(entity => entity.FullName)
            .NotEmpty().WithMessage(FULLNAME_IS_REQUIRED_MESSAGE);
    }
    
    public void AddRuleTaxDocumentShouldBeUnique(AbstractValidator<Entities.School> validator)
    {
        validator.RuleFor(entity => entity)
            .MustAsync(async (entity, cancellationToken) =>
            {
                var current = await repository.GetAsync(entity.Id, cancellationToken);
                if (current!.TaxDocument == entity.TaxDocument)
                {
                    return true;
                }
                
                var newOne = await repository.GetByTaxDocumentAsync(entity.TaxDocument, cancellationToken: cancellationToken);
                return newOne == null;
            })
            .WithMessage(TAXDOCUMENT_IS_UNIQUE_MESSAGE);
    }
}