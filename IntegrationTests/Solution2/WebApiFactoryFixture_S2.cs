using IntegrationTests.TestOutputLogging;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using PlayingWithTestHost;
using PlayingWithTestHost.Model;
using Xunit.Abstractions;

namespace IntegrationTests.Solution2;

#nullable enable
public sealed class WebApiFactoryFixture_S2 : WebApplicationFactory<Startup>
{
    public UserModel? TestUser { get; set; }

    public HttpClient HttpClient { get; private set; }

    public ITestOutputHelper? TestOutput { get; set; }

    public WebApiFactoryFixture_S2()
    {
        HttpClient = CreateClient();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(configureServices);

        builder.ConfigureLogging(configureLogging);
    }

    private void configureServices(IServiceCollection services)
    {
        //services.RemoveAll<IValueProvider>(); // This is not necessary, just to make sure.
        services.Replace(ServiceDescriptor.Singleton<IValueProvider, FakeValueProvider>());

        services.AddSingleton<IPolicyEvaluator>(_ => new FakeUserPolicyEvaluator(() => TestUser?.ToClaims()));
    }

    private void configureLogging(ILoggingBuilder logBuilder)
    {
        logBuilder.ClearProviders();

        //logBuilder.SetMinimumLevel(LogLevel.Error);

        logBuilder.AddTestOutputLogger(() => TestOutput);
    }
}
