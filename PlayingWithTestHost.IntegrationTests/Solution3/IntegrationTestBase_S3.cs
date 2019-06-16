using System.Net.Http;
using PlayingWithTestHost.Model;
using Xunit;

namespace PlayingWithTestHost.IntegrationTests.Solution3
{
  public class IntegrationTestBase_S3 : IClassFixture<WebApiFactory>
  {
    private readonly WebApiFactory _webApiFactory;

    protected UserModel _testUser { get => _webApiFactory.TestUser; set => _webApiFactory.TestUser = value; }

    protected readonly HttpClient _httpClient;

    public IntegrationTestBase_S3(WebApiFactory webApiFactory)
    {
      _webApiFactory = webApiFactory;

      _httpClient = _webApiFactory.CreateClient();
    }
  }
}
