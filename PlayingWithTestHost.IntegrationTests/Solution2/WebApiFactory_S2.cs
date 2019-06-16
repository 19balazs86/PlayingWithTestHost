using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.IntegrationTests.Solution2
{
  public class WebApiFactory_S2 : WebApplicationFactory<Startup>
  {
    public UserModel TestUser { get; set; }

    private readonly Func<UserModel> _testUserFunc;

    public WebApiFactory_S2()
    {
      _testUserFunc = () => TestUser;
    }

    protected override IWebHostBuilder CreateWebHostBuilder()
    {
      return WebHost
        .CreateDefaultBuilder()
        //.UseEnvironment(EnvironmentName.Development)
        .ConfigureTestServices(services =>
        {
          services.AddSingleton<IValueProvider, FakeValueProvider>();

          services.AddMvc(options =>
          {
            options.Filters.Add(new AllowAnonymousFilter());
            options.Filters.Add(new FakeUserFilter(_testUserFunc));
          });
        })
        .UseStartup<Startup>();
    }
  }
}
