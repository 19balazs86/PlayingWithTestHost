﻿using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithTestHost.Dummy;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.IntegrationTests
{
  public class TestServerFixture : IDisposable, ITestUser
  {
    public UserModel TestUser { get; set; }

    public HttpClient Client { get; }

    private readonly TestServer _testServer;

    public TestServerFixture()
    {
      // Use TestStartup class from the API Host project to configure the test server.
      // If you use Startup instead of TestStartup, you will have authentication issue.
      IWebHostBuilder builder = new WebHostBuilder()
        .ConfigureAppConfiguration(configBuilder => configBuilder.AddJsonFile("appsettings.json"))
        .ConfigureServices(services => services.AddSingleton<ITestUser>(this))
        .UseStartup<TestStartup>();

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
