using SchoolApp.CrossCutting.Validator;
using SchoolApp.Domain.Models.Authentication.UpdatePassword;

namespace SchoolApp.Domain.Validators.Authentication.Interfaces;

public interface IIsValidUpdatePasswordValidator : IValidator<UpdatePasswordModel>
{
}