using Application.Driven.Repositories;
using Core.Ports.Repositories.Article;
using Core.Ports.Repositories.User;

namespace Application.DependencyInjection.Repositories;

public static class RepositoriesRegistration
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IArticleRepository, ArticleRepository>();
            
        return services;
    }
}