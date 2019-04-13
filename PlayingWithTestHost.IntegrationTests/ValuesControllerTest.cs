using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PlayingWithTestHost.Model;
using Xunit;

namespace PlayingWithTestHost.IntegrationTests
{
  public class ValuesControllerTest : IClassFixture<TestServerFixture>
  {
    private readonly UserModel _user, _admin;

    private readonly TestServerFixture _fixture;

    public ValuesControllerTest(TestServerFixture fixture)
    {
      _fixture = fixture;

      _user  = new UserModel("Test user",  new[] { "User" });
      _admin = new UserModel("Test admin", new[] { "Admin" });
    }

    [Theory]
    [InlineData("values",         typeof(IEnumerable<string>))]
    [InlineData("values/config",  typeof(TestConfig))]
    [InlineData("values/user",    typeof(UserModel))]
    public async Task GetValues(string requestUri, Type objectType)
    {
      // Arrange
      _fixture.TestUser = _user;

      HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), requestUri);

      // Act
      HttpResponseMessage response = await _fixture.Client.SendAsync(request);
      
      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      object responseObject = await response.Content.ReadAsAsync(objectType);

      Assert.NotNull(responseObject);
    }

    [Fact]
    public async Task GetAdminUser_With_NonAdmin()
    {
      // Arrange
      _fixture.TestUser = _user;

      // Act
      HttpResponseMessage response = await _fixture.Client.GetAsync("values/admin");

      // Assert
      Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetAdminUser_With_Admin()
    {
      // Arrange
      _fixture.TestUser = _admin;

      // Act
      HttpResponseMessage response = await _fixture.Client.GetAsync("values/admin");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      UserModel userModel = await response.Content.ReadAsAsync<UserModel>();

      Assert.NotNull(userModel);
      Assert.Equal(_admin.Name, userModel.Name);
    }

    [Fact]
    public async Task Anonymous()
    {
      // Arrange
      _fixture.TestUser = null;

      // Act
      HttpResponseMessage response = await _fixture.Client.GetAsync("values/anonymous");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}
