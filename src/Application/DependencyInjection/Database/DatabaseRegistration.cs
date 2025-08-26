using Application.Driven.Database;
using Microsoft.EntityFrameworkCore;

namespace Application.DependencyInjection.Database;

public static class DatabaseRegistration
{
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlServer(
                connectionString, b => b.MigrationsAssembly("Application")); 
        });

        return services;
    }
}