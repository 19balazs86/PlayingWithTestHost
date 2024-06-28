using PlayingWithTestHost.Model;
using Xunit;

namespace IntegrationTests.Solution3;

public abstract class IntegrationTestBase_S3 : IClassFixture<WebApiFactoryFixture_S3>
{
    private readonly WebApiFactoryFixture_S3 _webApiFactory;

    protected UserModel _testUser { get => _webApiFactory.TestUser; set => _webApiFactory.TestUser = value; }

    protected HttpClient _httpClient => _webApiFactory.HttpClient;

    public IntegrationTestBase_S3(WebApiFactoryFixture_S3 webApiFactory)
    {
        _webApiFactory = webApiFactory;

        _testUser = null;
    }
}
