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
            services.AddTestAuthentication(() => _testUserProvider.TestUser?.ToClaims());
        }
    }
}
