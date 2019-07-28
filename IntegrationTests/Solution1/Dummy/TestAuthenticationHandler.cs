using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IntegrationTests.Solution1.Dummy
{
  public static class TestAuthenticationExtensions
  {
    public static AuthenticationBuilder AddTestAuth(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
    {
      return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(TestStartup.AuthScheme, configureOptions);
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
      ClaimsIdentity claimsIdentity = Options.Identity();

      if (claimsIdentity is null)
        return Task.FromResult(AuthenticateResult.Fail("ClaimsIdentity is null."));

      ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

      AuthenticationTicket authenticationTicket =
        new AuthenticationTicket(claimsPrincipal, new AuthenticationProperties(), TestStartup.AuthScheme);

      return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }
  }

  public class TestAuthenticationOptions : AuthenticationSchemeOptions
  {
    public Func<IEnumerable<Claim>> TestUserClaimsFunc { get; set; }

    public ClaimsIdentity Identity()
    {
      IEnumerable<Claim> claims = TestUserClaimsFunc?.Invoke();

      if (claims is null) return null;

      return new ClaimsIdentity(claims, TestStartup.AuthScheme);
    }
  }
}
