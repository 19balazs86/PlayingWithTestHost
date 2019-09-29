using System.Net.Http;
using IntegrationTests.Solution1.Dummy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlayingWithTestHost;
using PlayingWithTestHost.Model;

namespace IntegrationTests.Solution3
{
  public class WebApiFactory_S3 : WebApplicationFactory<Startup>
  {
    public UserModel TestUser { get; set; }

    public HttpClient HttpClient { get; private set; }

    public WebApiFactory_S3()
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
             services.AddSingleton<IValueProvider, FakeValueProvider>();

             services.AddAuthentication(options =>
             {
               options.DefaultAuthenticateScheme = TestStartup.AuthScheme;
               options.DefaultChallengeScheme    = TestStartup.AuthScheme;
             })
            .AddTestAuth(o => o.TestUserClaimsFunc = () => TestUser?.ToClaims());
           }));
    }
  }
}
