using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using PlayingWithTestHost.Dummy;

namespace PlayingWithTestHost.IntegrationTests
{
  public class TestServerFixture : IDisposable
  {
    private readonly TestServer _testServer;

    public HttpClient Client { get; }

    public TestServerFixture()
    {
      // Use TestStartup class from your API Host project to configure the test server.
      // If you use Startup instead of TestStartup, you will have authentication issue.
      IWebHostBuilder builder = new WebHostBuilder()
        .ConfigureAppConfiguration(configBuilder => configBuilder.AddJsonFile("appsettings.json"))
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
