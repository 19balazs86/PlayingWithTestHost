using IntegrationTests.Solution1.Dummy;
using PlayingWithTestHost;
using PlayingWithTestHost.Model;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace IntegrationTests.Solution1;

public sealed class ValuesControllerTest_S1 : IClassFixture<TestServerFixture>
{
    private readonly UserModel _user, _admin;

    private readonly TestServerFixture _fixture;

    public ValuesControllerTest_S1(TestServerFixture fixture)
    {
        _fixture = fixture;

        _fixture.TestUser = null;

        _user  = TestUsers.User;
        _admin = TestUsers.Admin;
    }

    [Theory]
    [InlineData("values",        typeof(IEnumerable<string>))]
    [InlineData("values/config", typeof(TestConfig))]
    [InlineData("values/user",   typeof(UserModel))]
    public async Task GetValues(string requestPath, Type objectType)
    {
        // Arrange
        _fixture.TestUser = _user;

        // Act
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync(requestPath);

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        object responseObject = await response.Content.ReadFromJsonAsync(objectType);

        Assert.NotNull(responseObject);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetValuesForUser(bool isAdmin)
    {
        // Arrange
        _fixture.TestUser = isAdmin ? _admin : _user;

        // Act
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync("values/user");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        UserModel userModel = await response.Content.ReadFromJsonAsync<UserModel>();

        Assert.NotNull(userModel);
        Assert.Equal(_fixture.TestUser.Name, userModel.Name);
    }

    [Fact]
    public async Task GetAdminUser_With_NonAdmin()
    {
        // Arrange
        _fixture.TestUser = _user;

        // Act
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync("values/admin");

        // Assert
        Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetAdminUser_With_Admin()
    {
        // Arrange
        _fixture.TestUser = _admin;

        // Act
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync("values/admin");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        UserModel userModel = await response.Content.ReadFromJsonAsync<UserModel>();

        Assert.NotNull(userModel);
        Assert.Equal(_admin.Name, userModel.Name);
    }

    [Fact]
    public async Task GetValueProvider()
    {
        // Arrange
        _fixture.TestUser = _user;

        // Act
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync("values/value-provider");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal(FakeValueProvider.Value, await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Anonymous()
    {
        // Arrange
        _fixture.TestUser = null;

        // Act
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync("values/anonymous");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task Response_Unauthorized()
    {
        // Arrange
        _fixture.TestUser = null;

        // Act
        HttpResponseMessage response = await _fixture.HttpClient.GetAsync("values/user");

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}
