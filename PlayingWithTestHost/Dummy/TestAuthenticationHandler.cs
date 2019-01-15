using System;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.Dummy
{
  public static class TestAuthenticationExtensions
  {
    public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
    {
      return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(TestStartup.AuthScheme, "Test Auth", configureOptions);
    }
  }

  public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
  {
    public TestAuthenticationHandler(
      IOptionsMonitor<TestAuthenticationOptions> options,
      ILoggerFactory logger,
      UrlEncoder encoder,
      ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
      ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(Options.Identity());

      AuthenticationTicket authenticationTicket =
        new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), TestStartup.AuthScheme);

      return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }
  }

  public class TestAuthenticationOptions : AuthenticationSchemeOptions
  {
    public Func<UserModel> TestUserFunc { get; set; }

    public ClaimsIdentity Identity() => new ClaimsIdentity(TestUserFunc().ToClaims(), "test");
  }
}
