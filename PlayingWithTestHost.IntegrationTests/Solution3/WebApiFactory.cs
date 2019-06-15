using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithTestHost.IntegrationTests.Solution1.Dummy;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.IntegrationTests.Solution3
{
  public class WebApiFactory : WebApplicationFactory<Startup>
  {
    public Func<UserModel> TestUserFunc { get; set; }

    protected override IWebHostBuilder CreateWebHostBuilder()
    {
      return WebHost
        .CreateDefaultBuilder()
        //.UseEnvironment(EnvironmentName.Development)
        .ConfigureTestServices(services =>
        {
          services.AddSingleton<IValueProvider, FakeValueProvider>();

          services.AddAuthentication(options =>
          {
            options.DefaultAuthenticateScheme = TestStartup.AuthScheme;
            options.DefaultChallengeScheme    = TestStartup.AuthScheme;
          })
          .AddTestAuth(o => o.TestUserFunc = TestUserFunc);
        })
        .UseStartup<Startup>();
    }
  }
}
