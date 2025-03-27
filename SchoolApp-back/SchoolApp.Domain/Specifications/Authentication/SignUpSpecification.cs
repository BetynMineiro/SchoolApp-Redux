using FluentValidation;
using SchoolApp.Domain.Models.Authentication.SignUp;
using SchoolApp.Domain.Specifications.Authentication.Interfaces;

namespace SchoolApp.Domain.Specifications.Authentication;

public class SignUpSpecification : ISignUpSpecification
{
    private const string NAME_IS_REQUIRED_MESSAGE = "Name is required";
    private const string EMAIL_IS_REQUIRED_MESSAGE = "Email is required";
    private const string PASSWORD_IS_REQUIRED_MESSAGE = "Password is required";
    private const string EMAIL_IS_VALID_MESSAGE = "Email needs to be valid";
    private const string PASSWORD_MIN_LENGTH_MESSAGE = "Password must be at least 8 characters long";
    private const string PASSWORD_UPPERCASE_MESSAGE = "Password must contain at least one uppercase letter";
    private const string PASSWORD_LOWERCASE_MESSAGE = "Password must contain at least one lowercase letter";
    private const string PASSWORD_NUMBER_MESSAGE = "Password must contain at least one number";
    private const string PASSWORD_SPECIAL_CHARACTER_MESSAGE = "Password must contain at least one special character";

    public void AddRuleEmailShouldNotEmpty(AbstractValidator<SignUpModel> validator)
    {
        validator.RuleFor(entity => entity.Email)
            .NotEmpty().WithMessage(EMAIL_IS_REQUIRED_MESSAGE);
    }

    public void AddRuleNameShouldNotEmpty(AbstractValidator<SignUpModel> validator)
    {
        validator.RuleFor(entity => entity.Name)
            .NotEmpty().WithMessage(NAME_IS_REQUIRED_MESSAGE);
    }

    public void AddRulePasswordShouldNotEmpty(AbstractValidator<SignUpModel> validator)
    {
        validator.RuleFor(entity => entity.Password)
            .NotEmpty().WithMessage(PASSWORD_IS_REQUIRED_MESSAGE);
    }

    public void AddRuleEmailShouldBeValid(AbstractValidator<SignUpModel> validator)
    {
        validator.RuleFor(entity => entity.Email)
            .EmailAddress().WithMessage(EMAIL_IS_VALID_MESSAGE);
    }

    public void AddRulePasswordShouldBeStrong(AbstractValidator<SignUpModel> validator)
    {
        validator.RuleFor(entity => entity.Password)
            .MinimumLength(8).WithMessage(PASSWORD_MIN_LENGTH_MESSAGE)
            .Matches("[A-Z]").WithMessage(PASSWORD_UPPERCASE_MESSAGE)
            .Matches("[a-z]").WithMessage(PASSWORD_LOWERCASE_MESSAGE)
            .Matches("[0-9]").WithMessage(PASSWORD_NUMBER_MESSAGE)
            .Matches("[^a-zA-Z0-9]").WithMessage(PASSWORD_SPECIAL_CHARACTER_MESSAGE);
    }
}