using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.Controllers
{
  [Route("[controller]")]
  [Authorize] // Here we have an Authorize attribute.
  [ApiController]
  public class ValuesController : ControllerBase
  {
    private readonly TestConfig _testConfig;

    public ValuesController(TestConfig testConfig) => _testConfig = testConfig;

    [HttpGet]
    public ActionResult<IEnumerable<string>> Get() => new string[] { "value1", "value2" };

    [HttpGet("config")]
    public ActionResult<TestConfig> GetConfigValues() => _testConfig;

    [HttpGet("user")]
    public ActionResult<UserModel> GetUserValues() => new UserModel(User.Claims);

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public ActionResult<UserModel> GetAdminUserValues() => new UserModel(User.Claims);

    [AllowAnonymous]
    [HttpGet("anonymous")]
    public ActionResult<UserModel> GetAnonymous() => Ok();
  }
}
