using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlayingWithTestHost;

namespace IntegrationTests.Solution1.Dummy
{
  public class TestStartup : Startup
  {
    public static readonly string AuthScheme = "TestAuthScheme";

    private readonly ITestUserProvider _testUserProvider;

    public TestStartup(IConfiguration configuration, ITestUserProvider testUserProvider) : base(configuration)
    {
      _testUserProvider = testUserProvider;
    }

    protected override void ConfigureAuthentication(IServiceCollection services)
    {
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = AuthScheme;
        options.DefaultChallengeScheme    = AuthScheme;
      })
      .AddTestAuth(o => o.TestUserClaimsFunc = () => _testUserProvider.TestUser?.ToClaims());
    }
  }
}
