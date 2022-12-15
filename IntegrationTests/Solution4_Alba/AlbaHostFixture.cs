using Alba;
using IntegrationTests.Solution1.Dummy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithTestHost.Model;
using Xunit;

namespace IntegrationTests.Solution4_Alba
{
    public class AlbaHostFixture : IAsyncLifetime
    {
        public IAlbaHost AlbaWebHost { get; set; }

        public UserModel TestUser { get; set; }

        public async Task InitializeAsync()
        {
            //var securityStub = new CustomAuthenticationStub(() => TestUser?.ToClaims());
            //AlbaWebHost = await AlbaHost.For<PlayingWithTestHost.Program>(configureWebHostBuilder, securityStub);

            AlbaWebHost = await AlbaHost.For<PlayingWithTestHost.Program>(configureWebHostBuilder);
        }

        private void configureWebHostBuilder(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.ConfigureTestServices(configureTestServices);
        }

        private void configureTestServices(IServiceCollection services)
        {
            services.AddTestAuthentication(configureAuthOptions);

            services.AddSingleton<PlayingWithTestHost.IValueProvider, FakeValueProvider>();
        }

        private void configureAuthOptions(TestAuthenticationOptions options)
        {
            options.TestUserClaimsFunc = () => TestUser?.ToClaims();
        }

        public async Task DisposeAsync()
        {
            await AlbaWebHost.DisposeAsync();
        }
    }
}
