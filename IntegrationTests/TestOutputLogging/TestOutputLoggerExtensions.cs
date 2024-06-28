using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace IntegrationTests.TestOutputLogging;

#nullable enable
public static class TestOutputLoggerExtensions
{
    public static IServiceCollection AddTestOutputLogger(this IServiceCollection services, Func<ITestOutputHelper?> getTestOutputFunc)
    {
        var loggerProvider = new TestOutputLoggerProvider(getTestOutputFunc);

        return services.AddSingleton<ILoggerProvider>(loggerProvider);
    }

    public static ILoggingBuilder AddTestOutputLogger(this ILoggingBuilder logBuilder, Func<ITestOutputHelper?> getTestOutputFunc)
    {
        var loggerProvider = new TestOutputLoggerProvider(getTestOutputFunc);

        return logBuilder.AddProvider(loggerProvider); // This line is technically: services.AddSingleton(provider)
    }
}
