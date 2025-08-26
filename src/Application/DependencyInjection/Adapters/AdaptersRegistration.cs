using Application.Driven.Adapter;
using Core.Ports.Services.NytNews;

namespace Application.DependencyInjection.Adapters;

public static class AdaptersRegistration
{
    public static IServiceCollection AddAdapters(this IServiceCollection services)
    {
        services.AddHttpClient<INytNewsPort, NytNewsAdapter>();
            
        return services;
    }
}