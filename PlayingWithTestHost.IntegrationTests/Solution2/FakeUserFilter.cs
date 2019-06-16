using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.IntegrationTests.Solution2
{
  public class FakeUserFilter : IAsyncActionFilter
  {
    private readonly Func<UserModel> _testUserFunc;

    public FakeUserFilter(Func<UserModel> testUserFunc)
      => _testUserFunc = testUserFunc;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      IEnumerable<Claim> claims = _testUserFunc?.Invoke()?.ToClaims() ?? Enumerable.Empty<Claim>();

      var claimsIdentity = new ClaimsIdentity(claims);

      context.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

      await next();
    }
  }
}
