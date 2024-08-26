using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace IntegrationTests;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection ReplaceWithSingletonExt<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        return services.Replace(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Singleton));
    }

    public static IServiceCollection ReplaceWithSingletonExt<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        return services.Replace(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), ServiceLifetime.Singleton));
    }

    public static IServiceCollection ReplaceWithScopedExt<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        return services.Replace(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Scoped));
    }

    public static IServiceCollection ReplaceWithScopedExt<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        return services.Replace(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), ServiceLifetime.Scoped));
    }

    public static IServiceCollection ReplaceWithTransientExt<TService, TImplementation>(this IServiceCollection services)
        where TService : class
        where TImplementation : class, TService
    {
        return services.Replace(new ServiceDescriptor(typeof(TService), typeof(TImplementation), ServiceLifetime.Transient));
    }

    public static IServiceCollection ReplaceWithTransientExt<TImplementation>(this IServiceCollection services)
        where TImplementation : class
    {
        return services.Replace(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), ServiceLifetime.Transient));
    }
}