using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PlayingWithTestHost.Model;
using Xunit;

namespace PlayingWithTestHost.IntegrationTests.Solution1
{
  public class ValuesControllerTest_S1 : IntegrationTestBase_S1
  {
    private readonly UserModel _user, _admin;

    public ValuesControllerTest_S1()
    {
      _user  = new UserModel("Test user",  new[] { "User" });
      _admin = new UserModel("Test admin", new[] { "Admin" });
    }

    [Theory]
    [InlineData("values",        typeof(IEnumerable<string>))]
    [InlineData("values/config", typeof(TestConfig))]
    [InlineData("values/user",   typeof(UserModel))]
    public async Task GetValues(string requestPath, Type objectType)
    {
      // Arrange
      TestUser = _user;
      
      // Act
      HttpResponseMessage response = await _client.GetAsync(requestPath);
      
      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      object responseObject = await response.Content.ReadAsAsync(objectType);

      Assert.NotNull(responseObject);
    }

    [Fact]
    public async Task GetAdminUser_With_NonAdmin()
    {
      // Arrange
      TestUser = _user;

      // Act
      HttpResponseMessage response = await _client.GetAsync("values/admin");

      // Assert
      Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task GetAdminUser_With_Admin()
    {
      // Arrange
      TestUser = _admin;

      // Act
      HttpResponseMessage response = await _client.GetAsync("values/admin");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      UserModel userModel = await response.Content.ReadAsAsync<UserModel>();

      Assert.NotNull(userModel);
      Assert.Equal(_admin.Name, userModel.Name);
    }

    [Fact]
    public async Task GetValueProvider()
    {
      // Arrange
      TestUser = _user;

      // Act
      HttpResponseMessage response = await _client.GetAsync("values/value-provider");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.Equal(FakeValueProvider.Value, await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Anonymous()
    {
      // Arrange
      TestUser = null;

      // Act
      HttpResponseMessage response = await _client.GetAsync("values/anonymous");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}
