using SchoolApp.CrossCutting.Validator;
using SchoolApp.Domain.Models.Authentication.SignUp;

namespace SchoolApp.Domain.Validators.Authentication.Interfaces;

public interface IIsValidSignUpValidator : IValidator<SignUpModel>
{
}