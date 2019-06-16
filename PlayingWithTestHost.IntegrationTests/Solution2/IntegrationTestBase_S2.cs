using System.Net.Http;
using PlayingWithTestHost.Model;
using Xunit;

namespace PlayingWithTestHost.IntegrationTests.Solution2
{
  public class IntegrationTestBase_S2 : IClassFixture<WebApiFactory_S2>
  {
    protected readonly WebApiFactory_S2 _webApiFactory;

    protected UserModel _testUser { get => _webApiFactory.TestUser; set => _webApiFactory.TestUser = value; }

    protected HttpClient _httpClient  => _webApiFactory.HttpClient;

    public IntegrationTestBase_S2(WebApiFactory_S2 webApiFactory)
    {
      _webApiFactory = webApiFactory;
    }

    //protected virtual HttpClient createClientFor(
    //  UserModel user = null,
    //  WebApplicationFactoryClientOptions clientOptions = null)
    //{
    //  // In the previous solution the _factory was WebApplicationFactory<Startup>
    //  // and the base class : IClassFixture<WebApplicationFactory<Startup>>

    //  WebApplicationFactory<Startup> factory = _factory.WithWebHostBuilder(builder =>
    //  {
    //    // Services can be overridden in a test with a call to ConfigureTestServices on the host builder.

    //    builder
    //      //.UseEnvironment(EnvironmentName.Development)
    //      //.ConfigureAppConfiguration(configBuilder => configBuilder.AddJsonFile("appsettings.json"))
    //      .ConfigureTestServices(services =>
    //      {
    //        services.AddSingleton<IValueProvider, FakeValueProvider>();

    //        if (user is null) return;

    //        services.AddMvc(options =>
    //        {
    //          options.Filters.Add(new AllowAnonymousFilter());
    //          options.Filters.Add(new FakeUserFilter(user));
    //        });
    //        //.AddApplicationPart(typeof(Startup).Assembly);
    //      });
    //  });

    //  return clientOptions is null ? factory.CreateClient() : factory.CreateClient(clientOptions);
    //}
  }
}
