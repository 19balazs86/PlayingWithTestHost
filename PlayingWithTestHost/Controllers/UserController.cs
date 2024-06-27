using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayingWithTestHost.Model;
using System.Security.Claims;

namespace PlayingWithTestHost.Controllers;

[Route("[controller]")]
[ApiController]
public sealed class UserController : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (loginModel is not { Name: "test", Password: "pass" })
        {
            return Unauthorized();
        }

        var user = new UserModel(loginModel.Name, ["User"]);

        var claimsIdentity = new ClaimsIdentity(user.ToClaims(), Startup.DefaultAuthScheme);

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        await HttpContext.SignInAsync(Startup.DefaultAuthScheme, claimsPrincipal);

        return Ok();
    }

    [HttpGet("logout")]
    public Task Logout()
    {
        return HttpContext.SignOutAsync(Startup.DefaultAuthScheme);
    }
}