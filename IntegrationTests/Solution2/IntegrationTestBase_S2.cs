using PlayingWithTestHost.Model;
using Xunit;
using Xunit.Abstractions;

namespace IntegrationTests.Solution2;

public abstract class IntegrationTestBase_S2 : IClassFixture<WebApiFactory_S2>
{
    protected readonly WebApiFactory_S2 _webApiFactory;

    protected UserModel _testUser { get => _webApiFactory.TestUser; set => _webApiFactory.TestUser = value; }

    protected HttpClient _httpClient => _webApiFactory.HttpClient;

    public IntegrationTestBase_S2(WebApiFactory_S2 webApiFactory, ITestOutputHelper testOutput)
    {
        _webApiFactory = webApiFactory;

        _webApiFactory.TestOutput = testOutput;
    }
}
