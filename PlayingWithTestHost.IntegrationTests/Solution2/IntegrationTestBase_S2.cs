using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithTestHost.Model;
using Xunit;

namespace PlayingWithTestHost.IntegrationTests.Solution2
{
  public class IntegrationTestBase_S2 : IClassFixture<WebApplicationFactory<Startup>>
  {
    protected readonly WebApplicationFactory<Startup> _factory;

    public IntegrationTestBase_S2(WebApplicationFactory<Startup> factory)
    {
      _factory = factory;
    }

    protected virtual HttpClient createClientFor(
      UserModel user = null,
      WebApplicationFactoryClientOptions clientOptions = null)
    {
      WebApplicationFactory<Startup> factory = _factory.WithWebHostBuilder(builder =>
      {
        // Services can be overridden in a test with a call to ConfigureTestServices on the host builder.

        builder
          //.UseEnvironment(EnvironmentName.Development)
          //.ConfigureAppConfiguration(configBuilder => configBuilder.AddJsonFile("appsettings.json"))
          .ConfigureTestServices(services =>
          {
            services.AddSingleton<IValueProvider, FakeValueProvider>();

            if (user is null) return;

            services.AddMvc(options =>
            {
              options.Filters.Add(new AllowAnonymousFilter());
              options.Filters.Add(new FakeUserFilter(user));
            });
            //.AddApplicationPart(typeof(Startup).Assembly);
          });
      });

      return clientOptions is null ? factory.CreateClient() : factory.CreateClient(clientOptions);
    }
  }
}
