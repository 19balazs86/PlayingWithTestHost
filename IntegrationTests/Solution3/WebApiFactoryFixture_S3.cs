using IntegrationTests.Solution1.Dummy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using PlayingWithTestHost;
using PlayingWithTestHost.Model;

namespace IntegrationTests.Solution3;

public class WebApiFactoryFixture_S3 : WebApplicationFactory<Startup>
{
    public UserModel TestUser { get; set; }

    public HttpClient HttpClient { get; private set; }

    public WebApiFactoryFixture_S3()
    {
        HttpClient = CreateClient();
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            //services.RemoveAll<IValueProvider>(); // This is not necessary, just to make sure.
            services.ReplaceWithSingletonExt<IValueProvider, FakeValueProvider>();

            services.AddTestAuthentication(() => TestUser?.ToClaims());
        });
    }
}
