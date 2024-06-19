using Alba;
using PlayingWithTestHost;
using PlayingWithTestHost.Model;
using System.Net;
using Xunit;

namespace IntegrationTests.Solution4_Alba;

public sealed class ValuesControllerTest_S4_Alba : IClassFixture<AlbaHostFixture>
{
    private readonly AlbaHostFixture _fixture;

    private readonly IAlbaHost _albaHost;

    private readonly UserModel _user, _admin;

    public ValuesControllerTest_S4_Alba(AlbaHostFixture albaHostFixture)
    {
        _fixture = albaHostFixture;
        _albaHost = albaHostFixture.AlbaWebHost;

        _user  = new UserModel("Test user",  ["User"]);
        _admin = new UserModel("Test admin", ["Admin"]);

        _fixture.TestUser = _user;
    }

    [Fact]
    public async Task GetValueProvider()
    {
        // Act
        string responseValue = await _albaHost.GetAsText("/values/value-provider");

        // Assert
        Assert.Equal(FakeValueProvider.Value, responseValue);
    }

    [Fact]
    public async Task GetTestConfig()
    {
        // Act
        TestConfig testConfig = await _albaHost.GetAsJson<TestConfig>("/values/config");

        // Assert
        Assert.NotNull(testConfig);
        Assert.Equal(AlbaHostFixture.TestConfigValue, testConfig.Key1);
        Assert.True(testConfig.Key2);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetValuesForUser(bool isAdmin)
    {
        // Arrange
        _fixture.TestUser = isAdmin ? _admin : _user;

        // Act
        UserModel userModel = await _albaHost.GetAsJson<UserModel>("/values/user");

        // Assert
        Assert.NotNull(userModel);
        Assert.Equal(_fixture.TestUser.Name, userModel.Name);
        Assert.True(Enumerable.SequenceEqual(_fixture.TestUser.Roles, userModel.Roles));
    }

    [Fact]
    public async Task GetAdminUser_With_Admin()
    {
        // Arrange
        _fixture.TestUser = _admin;

        // Act
        UserModel userModel = await _albaHost.GetAsJson<UserModel>("/values/admin");

        // Assert
        Assert.NotNull(userModel);
        Assert.Equal(_fixture.TestUser.Name, userModel.Name);
        Assert.True(Enumerable.SequenceEqual(_fixture.TestUser.Roles, userModel.Roles));
    }

    [Fact]
    public async Task GetAdminUser_With_NonAdmin()
    {
        // Arrange
        _fixture.TestUser = _user;

        // Act + Assert
        _ = await _albaHost.Scenario(scenario =>
        {
            scenario.Get.Url("/values/admin");
            scenario.StatusCodeShouldBe(HttpStatusCode.Forbidden);
        });
    }

    [Fact]
    public async Task Anonymous()
    {
        // Arrange
        _fixture.TestUser = null;

        // Act + Assert
        _ = await _albaHost.Scenario(scenario =>
        {
            scenario.Get.Url("/values/anonymous");
            scenario.StatusCodeShouldBeOk();
        });
    }

    [Fact]
    public async Task Response_Unauthorized()
    {
        // Arrange
        _fixture.TestUser = null;

        // Act + Assert
        _ = await _albaHost.Scenario(scenario =>
        {
            scenario.Get.Url("/values/user");
            scenario.StatusCodeShouldBe(HttpStatusCode.Unauthorized);
        });
    }
}
