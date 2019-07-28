using System.Net.Http;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlayingWithTestHost;
using PlayingWithTestHost.Model;

namespace IntegrationTests.Solution2
{
  public class WebApiFactory_S2 : WebApplicationFactory<Startup>
  {
    public UserModel TestUser { get; set; }

    public HttpClient HttpClient { get; private set; }

    public WebApiFactory_S2()
    {
      HttpClient = CreateClient();
    }

    protected override IHostBuilder CreateHostBuilder()
    {
      return Host
        .CreateDefaultBuilder()
        //.UseEnvironment(Environments.Development)
        .ConfigureWebHostDefaults(webHostBuilder =>
          webHostBuilder
            .UseStartup<Startup>() // The order is matter.
            .ConfigureTestServices(services =>
            {
              services
                .AddSingleton<IValueProvider, FakeValueProvider>()
                .AddSingleton<IPolicyEvaluator>(_ => new FakeUserPolicyEvaluator(() => TestUser?.ToClaims()));
            }));
    }
  }
}
