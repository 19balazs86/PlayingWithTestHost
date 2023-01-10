using Alba;
using IntegrationTests.Solution1.Dummy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PlayingWithTestHost.Model;
using Xunit;

namespace IntegrationTests.Solution4_Alba;

public class AlbaHostFixture : IAsyncLifetime
{
    public const string TestConfigValue = "OverriddenValue";

    public IAlbaHost AlbaWebHost { get; set; }

    public UserModel TestUser { get; set; }

    public async Task InitializeAsync()
    {
        //var securityStub = new CustomAuthenticationStub(() => TestUser?.ToClaims());
        //AlbaWebHost = await AlbaHost.For<PlayingWithTestHost.Program>(configureWebHostBuilder, securityStub);

        AlbaWebHost = await AlbaHost.For<PlayingWithTestHost.Program>(configureWebHostBuilder);

        // You can access services
        //var config = AlbaWebHost.Services.GetRequiredService<IOptions<PlayingWithTestHost.TestConfig>>();
    }

    private void configureWebHostBuilder(IWebHostBuilder webHostBuilder)
    {
        webHostBuilder.ConfigureTestServices(configureTestServices);

        webHostBuilder.ConfigureAppConfiguration(configureAppConfiguration);
    }

    private void configureTestServices(IServiceCollection services)
    {
        services.AddTestAuthentication(() => TestUser?.ToClaims());

        services.Replace(ServiceDescriptor.Singleton<PlayingWithTestHost.IValueProvider, FakeValueProvider>());
    }

    private static void configureAppConfiguration(IConfigurationBuilder configurationBuilder)
    {
        var configurationOverridden = new Dictionary<string, string>
        {
            ["TestConfig:Key1"] = TestConfigValue
        };

        configurationBuilder.AddInMemoryCollection(configurationOverridden);
    }

    public async Task DisposeAsync()
    {
        await AlbaWebHost.DisposeAsync();
    }
}
