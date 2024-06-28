using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace IntegrationTests.TestOutputLogging;

#nullable enable
public sealed class TestOutputLogger : ILogger
{
    private readonly string _categoryName;

    private readonly Func<ITestOutputHelper?> _getTestOutputFunc;

    private ITestOutputHelper? _testOutputHelper;

    public TestOutputLogger(string categoryName, Func<ITestOutputHelper?> getTestOutputFunc)
    {
        _categoryName = categoryName;

        _getTestOutputFunc = getTestOutputFunc;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        _testOutputHelper ??= _getTestOutputFunc(); // Only get it once, so if we have the output, just use it

        if (_testOutputHelper is null)
        {
            return;
        }

        string message = $"<{logLevel}> - [{_categoryName}] - {state}";

        if (exception is null)
        {
            _testOutputHelper.WriteLine(message);
        }
        else
        {
            _testOutputHelper.WriteLine($"{message}\n{exception}");
        }
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return default;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }
}
