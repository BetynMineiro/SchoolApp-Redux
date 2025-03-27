using Microsoft.Extensions.DependencyInjection;
using SchoolApp.Domain.DomainServices.Users;
using SchoolApp.Domain.Specifications.Authentication;
using SchoolApp.Domain.Specifications.Authentication.Interfaces;
using SchoolApp.Domain.Specifications.School;
using SchoolApp.Domain.Specifications.School.Interfaces;
using SchoolApp.Domain.Specifications.User;
using SchoolApp.Domain.Specifications.User.Interfaces;
using SchoolApp.Domain.Validators.Authentication;
using SchoolApp.Domain.Validators.Authentication.Interfaces;
using SchoolApp.Domain.Validators.School;
using SchoolApp.Domain.Validators.School.Interfaces;
using SchoolApp.Domain.Validators.User;
using SchoolApp.Domain.Validators.User.Interfaces;

namespace SchoolApp.Domain;

public static class DomainModule
{
    public static void ConfigureDomainLayer(this IServiceCollection services)
    {
        // Specifications
        services.AddScoped<ISignUpSpecification, SignUpSpecification>();
        services.AddScoped<IUpdatePasswordSpecification, UpdatePasswordSpecification>();
        services.AddScoped<ICreateSchoolSpecification, CreateSchoolSpecification>();
        services.AddScoped<IUpdateSchoolSpecification, UpdateSchoolSpecification>();
        services.AddScoped<ICreateUserSpecification, CreateUserSpecification>();
        services.AddScoped<IUpdateUserSpecification, UpdateUserSpecification>();
        
        //Validators
        services.AddScoped<IIsValidSignUpValidator, IsValidSignUpValidator>();
        services.AddScoped<IIsValidUpdatePasswordValidator, IsValidUpdatePasswordValidator>();
        services.AddScoped<IIsValidCreateSchoolValidator, IsValidCreateSchoolValidator>();
        services.AddScoped<IIsValidCreateUserValidator, IsValidCreateUserValidator>();
        services.AddScoped<IIsValidUpdateSchoolValidator, IsValidUpdateSchoolValidator>();
        services.AddScoped<IIsValidUpdateUserValidator, IsValidUpdateUserValidator>();

        //Services
        services.AddScoped<UserServiceContext>();
    }
}