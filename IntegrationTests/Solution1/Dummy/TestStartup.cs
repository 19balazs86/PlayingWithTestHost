using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithTestHost;

namespace IntegrationTests.Solution1.Dummy
{
    public class TestStartup : Startup
    {
        private readonly ITestUserProvider _testUserProvider;

        public TestStartup(IConfiguration configuration, ITestUserProvider testUserProvider) : base(configuration)
        {
            _testUserProvider = testUserProvider;
        }

        protected override void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddTestAuthentication(configureOptions);
        }

        private void configureOptions(TestAuthenticationOptions options)
        {
            options.TestUserClaimsFunc = () => _testUserProvider.TestUser?.ToClaims();
        }
    }
}
