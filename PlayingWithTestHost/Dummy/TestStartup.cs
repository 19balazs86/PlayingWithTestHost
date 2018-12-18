using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlayingWithTestHost.Dummy
{
  public class TestStartup : Startup
  {
    public static readonly string AuthScheme = "Test Scheme";

    public TestStartup(IConfiguration configuration) : base(configuration)
    {
    }

    protected override void ConfigureAuthentication(IServiceCollection services)
    {
      services.AddAuthentication(options =>
      {
        options.DefaultAuthenticateScheme = AuthScheme;
        options.DefaultChallengeScheme    = AuthScheme;
      })
      .AddTestAuth(o => { });
    }
  }
}
