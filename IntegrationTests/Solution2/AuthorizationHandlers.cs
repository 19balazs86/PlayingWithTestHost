using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace IntegrationTests.Solution2
{
  public class AllowAnonymousRequirement : IAuthorizationRequirement { }

  public class AllowAnonymousAuthorizationHandler : AuthorizationHandler<AllowAnonymousRequirement>
  {
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      AllowAnonymousRequirement requirement)
    {
      context.Succeed(requirement);

      return Task.CompletedTask;
    }
  }

  public class RolesAuthorizationHandler : AuthorizationHandler<RolesAuthorizationRequirement>
  {
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      RolesAuthorizationRequirement requirement)
    {
      context.Succeed(requirement);

      return Task.CompletedTask;
    }
  }
}
