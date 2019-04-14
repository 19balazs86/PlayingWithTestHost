﻿using System;
using System.Net.Http;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithTestHost.IntegrationTests.Solution1.Dummy;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.IntegrationTests.Solution1
{
  public class TestServerFixture : IDisposable, ITestUserProvider
  {
    public UserModel TestUser { get; set; }

    public HttpClient Client { get; }

    private readonly TestServer _testServer;

    public TestServerFixture()
    {
      // Services can be overridden in a test with a call to ConfigureTestServices on the host builder.

      // Use TestStartup class from the API Host project to configure the test server.
      // If you use Startup instead of TestStartup, you will have authentication issue.
      IWebHostBuilder builder = new WebHostBuilder()
        //.UseEnvironment(EnvironmentName.Development)
        .ConfigureAppConfiguration(configBuilder => configBuilder.AddJsonFile("appsettings.json"))
        .ConfigureServices(services => services.AddSingleton<ITestUserProvider>(this))
        .ConfigureTestServices(services => services.AddSingleton<IValueProvider, FakeValueProvider>())
        .UseStartup<TestStartup>()
        .UseSetting(WebHostDefaults.ApplicationKey, typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
      // Important: UseSetting after UseStartup. Otherwise, the request run on not found.

      _testServer = new TestServer(builder);

      Client = _testServer.CreateClient();
    }

    public void Dispose()
    {
      Client.Dispose();

      _testServer.Dispose();
    }
  }
}
