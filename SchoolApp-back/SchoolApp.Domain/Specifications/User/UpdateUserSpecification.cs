using SchoolApp.CrossCutting.Enum.Common;
using SchoolApp.CrossCutting.Extensions;
using FluentValidation;
using SchoolApp.Domain.Enum.Management;
using SchoolApp.Domain.Repositories;
using SchoolApp.Domain.Specifications.User.Interfaces;

namespace SchoolApp.Domain.Specifications.User;

public class UpdateUserSpecification(IUserRepository repository) : IUpdateUserSpecification
{
    private const string MANAGED_SCHOOL_ID_IS_REQUIRED_MESSAGE = "Managed School Id is required";
    private const string TAXDOCUMENT_IS_REQUIRED_MESSAGE = "Tax document is required";
    private const string FULLNAME_IS_REQUIRED_MESSAGE = "Full Name is required";
    private const string TAXDOCUMENT_IS_VALID_MESSAGE = "Tax document needs to be valid";
    private const string TAXDOCUMENT_IS_UNIQUE_MESSAGE = "Tax document needs to be unique";
    private const string PERSON_TYPE_ID_IS_REQUIRED_MESSAGE = "Person type Id is required";
    
    public void AddRuleTaxDocumentShouldNotEmpty(AbstractValidator<Entities.User> validator)
    {
        validator.RuleFor(entity => entity.TaxDocument)
            .NotEmpty().WithMessage(TAXDOCUMENT_IS_REQUIRED_MESSAGE);
    }
    public void AddRulePersonTypeShouldNotEmpty(AbstractValidator<Entities.User> validator)
    {
        validator.RuleFor(entity => entity.PersonType).IsInEnum().WithMessage(PERSON_TYPE_ID_IS_REQUIRED_MESSAGE);
    }

    public void AddRuleTaxDocumentShouldBeValid(AbstractValidator<Entities.User> validator)
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

    public void AddRuleFullNameShouldNotEmpty(AbstractValidator<Entities.User> validator)
    {
        validator.RuleFor(entity => entity.FullName)
            .NotEmpty().WithMessage(FULLNAME_IS_REQUIRED_MESSAGE);
    }


    public void AddRuleManagedSchoolIdShouldAdded(AbstractValidator<Entities.User> validator)
    {
        validator.When(
            x => x.ProfileType == ProfileType.SchoolManager,
            () =>
            {
                validator.RuleFor(entity => entity.ManagedSchoolId).NotEmpty()
                    .WithMessage(MANAGED_SCHOOL_ID_IS_REQUIRED_MESSAGE);
            });
    }
    
    public void AddRuleTaxDocumentShouldBeUnique(AbstractValidator<Entities.User> validator)
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