using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using Xunit.Abstractions;

namespace IntegrationTests.TestOutputLogging;

/* The idea came from this solution: https://www.nuget.org/packages/Meziantou.Extensions.Logging.Xunit
 * I had an issue applying the LoggerProvider to the WebApplicationFactory, which I use as a fixture
 * I could not pass the ITestOutputHelper because the fixture cannot have constructor parameters, and it is created before the test constructor runs
 * I created my own solution using a Func delegate to acquire the ITestOutputHelper in the TestOutputLogger
 *
 * Resources:
 * - Implement a custom logging provider: https://learn.microsoft.com/en-us/dotnet/core/extensions/custom-logging-provider
 * - Using an Event with a delegate: https://github.com/dotnet/AspNetCore.Docs/tree/main/aspnetcore/grpc/test-services/sample/Tests/Server/IntegrationTests/Helpers
 */

#nullable enable
public sealed class TestOutputLoggerProvider : ILoggerProvider
{
    private readonly ConcurrentDictionary<string, TestOutputLogger> _loggerDictionary;

    private readonly Func<ITestOutputHelper?> _getTestOutputFunc;

    public TestOutputLoggerProvider(Func<ITestOutputHelper?> getTestOutputFunc)
    {
        _loggerDictionary = new ConcurrentDictionary<string, TestOutputLogger>(StringComparer.OrdinalIgnoreCase);

        _getTestOutputFunc = getTestOutputFunc;
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggerDictionary.GetOrAdd(categoryName, catName => new TestOutputLogger(catName, _getTestOutputFunc));
    }

    public void Dispose()
    {
        _loggerDictionary.Clear();
    }
}
