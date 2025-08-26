using Application.Driven.Auth;
using Application.Driven.Token;
using Core.Ports.Auth;
using Core.Ports.Services.News;
using Core.Ports.Services.User;
using Core.Ports.Token;
using Core.Services.Auth;
using Core.Services.News;
using Core.Services.User;

namespace Application.DependencyInjection.Services;

public static class ServicesRegistration
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<INewsService, NewsService>();
        services.AddScoped<IUserArticlesService, UserArticlesService>();
            
        services.AddScoped<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ITokenService, TokenService>();
            
        return services;
    }
}