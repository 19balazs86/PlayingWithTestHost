using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Filters;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.IntegrationTests.Solution2
{
  public class FakeUserFilter : IAsyncActionFilter
  {
    private readonly UserModel _userModel;

    public FakeUserFilter(UserModel userModel) => _userModel = userModel;

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
      var claimsIdentity = new ClaimsIdentity(_userModel.ToClaims());

      context.HttpContext.User = new ClaimsPrincipal(claimsIdentity);

      await next();
    }
  }
}
