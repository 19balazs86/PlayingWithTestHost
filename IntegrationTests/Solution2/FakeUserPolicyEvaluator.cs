using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace IntegrationTests.Solution2
{
  public class FakeUserPolicyEvaluator : IPolicyEvaluator
  {
    private readonly Func<IEnumerable<Claim>> _testUserClaimsFunc;

    public FakeUserPolicyEvaluator(Func<IEnumerable<Claim>> testUserClaimsFunc)
      => _testUserClaimsFunc = testUserClaimsFunc;

    public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy policy, HttpContext context)
    {
      IEnumerable<Claim> claims = _testUserClaimsFunc?.Invoke() ?? Enumerable.Empty<Claim>();

      context.User = new ClaimsPrincipal(new ClaimsIdentity(claims));

      var authenticationTicket = new AuthenticationTicket(context.User, "context.User");

      return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }

    public Task<PolicyAuthorizationResult> AuthorizeAsync(
        AuthorizationPolicy policy,
        AuthenticateResult authenticationResult,
        HttpContext context,
        object resource)
    {
      return Task.FromResult(PolicyAuthorizationResult.Success());
    }
  }
}
