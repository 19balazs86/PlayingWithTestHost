using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PlayingWithTestHost.Model;

namespace PlayingWithTestHost.Controllers;

[Route("[controller]")]
//[Authorize] // Here we have an Authorize attribute.
[ApiController]
public sealed class ValuesController : ControllerBase
{
    private readonly TestConfig _testConfig;

    private readonly IValueProvider _valueProvider;

    public ValuesController(IOptions<TestConfig> testConfig, IValueProvider valueProvider)
    {
        _testConfig    = testConfig.Value;
        _valueProvider = valueProvider;
    }

    [HttpGet]
    public IEnumerable<string> Get() => ["value1", "value2"];

    [HttpGet("config")]
    public TestConfig GetConfigValues() => _testConfig;

    [HttpGet("user")]
    public UserModel GetUserValues() => UserModel.CreateFromClaims(User.Claims);

    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public UserModel GetAdminUserValues() => UserModel.CreateFromClaims(User.Claims);

    [HttpGet("value-provider")]
    public string GetValueProvider() => _valueProvider.GetValue();

    [AllowAnonymous]
    [HttpGet("anonymous")]
    public IActionResult GetAnonymous() => Ok();
}
