using SchoolApp.Domain.Models.Authentication.SignUp;
using FluentValidation;
using SchoolApp.Domain.Models.Authentication.UpdatePassword;
using SchoolApp.Domain.Specifications.Authentication.Interfaces;

namespace SchoolApp.Domain.Specifications.Authentication;

public class UpdatePasswordSpecification : IUpdatePasswordSpecification
{
    private const string ID_IS_REQUIRED_MESSAGE = "ID is required";
    private const string PASSWORD_IS_REQUIRED_MESSAGE = "Password is required";
    private const string PASSWORD_MIN_LENGTH_MESSAGE = "Password must be at least 8 characters long";
    private const string PASSWORD_UPPERCASE_MESSAGE = "Password must contain at least one uppercase letter";
    private const string PASSWORD_LOWERCASE_MESSAGE = "Password must contain at least one lowercase letter";
    private const string PASSWORD_NUMBER_MESSAGE = "Password must contain at least one number";
    private const string PASSWORD_SPECIAL_CHARACTER_MESSAGE = "Password must contain at least one special character";

    public void AddRuleIdShouldNotEmpty(AbstractValidator<UpdatePasswordModel> validator)
    {
        validator.RuleFor(entity => entity.Id)
            .NotEmpty().WithMessage(ID_IS_REQUIRED_MESSAGE);
    }

    public void AddRulePasswordShouldNotEmpty(AbstractValidator<UpdatePasswordModel> validator)
    {
        validator.RuleFor(entity => entity.Password)
            .NotEmpty().WithMessage(PASSWORD_IS_REQUIRED_MESSAGE);
    }

    public void AddRulePasswordShouldBeStrong(AbstractValidator<UpdatePasswordModel> validator)
    {
        validator.RuleFor(entity => entity.Password)
            .MinimumLength(8).WithMessage(PASSWORD_MIN_LENGTH_MESSAGE)
            .Matches("[A-Z]").WithMessage(PASSWORD_UPPERCASE_MESSAGE)
            .Matches("[a-z]").WithMessage(PASSWORD_LOWERCASE_MESSAGE)
            .Matches("[0-9]").WithMessage(PASSWORD_NUMBER_MESSAGE)
            .Matches("[^a-zA-Z0-9]").WithMessage(PASSWORD_SPECIAL_CHARACTER_MESSAGE);
    }
}