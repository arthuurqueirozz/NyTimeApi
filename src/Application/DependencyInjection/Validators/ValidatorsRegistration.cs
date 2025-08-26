using Application.Driving.Validators;
using FluentValidation;

namespace Application.DependencyInjection.Validators;

public static class ValidatorsRegistration
{
    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssemblyContaining<LoginRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();
        services.AddValidatorsFromAssemblyContaining<SaveArticleRequestValidator>();
        return services;
    }
}