using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlayingWithTestHost.IntegrationTests.Dummy
{
  public class TestStartup : Startup
  {
    public static readonly string AuthScheme = "Test Scheme";

    private readonly ITestUser _testUser;

    public TestStartup(IConfiguration configuration, ITestUser testUser) : base(configuration)
    {
      _testUser = testUser;
    }

    protected override void ConfigureAuthentication(IServiceCollection services)
    {
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = AuthScheme;
        options.DefaultChallengeScheme    = AuthScheme;
      })
      .AddTestAuth(o => o.TestUserFunc = () => _testUser.TestUser);
    }
  }
}
