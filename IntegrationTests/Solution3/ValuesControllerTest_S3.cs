using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PlayingWithTestHost;
using PlayingWithTestHost.Model;
using Xunit;

namespace IntegrationTests.Solution3
{
  public class ValuesControllerTest_S3 : IntegrationTestBase_S3
  {
    private readonly UserModel _user, _admin;

    public ValuesControllerTest_S3(WebApiFactory_S3 webApiFactory) : base(webApiFactory)
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
      _testUser = _user;
      
      // Act
      HttpResponseMessage response = await _httpClient.GetAsync(requestPath);
      
      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      object responseObject = await response.Content.ReadAsAsync(objectType);

      Assert.NotNull(responseObject);
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public async Task GetValuesForUser(bool isAdmin)
    {
      // Arrange
      _testUser = isAdmin ? _admin : _user;

      // Act
      HttpResponseMessage response = await _httpClient.GetAsync("values/user");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      UserModel userModel = await response.Content.ReadAsAsync<UserModel>();

      Assert.NotNull(userModel);
      Assert.Equal(_testUser.Name, userModel.Name);
    }

    // This will fail: Authentication mechanism is overwritten in IntegrationTestBase.
    //[Fact]
    //public async Task GetAdminUser_With_NonAdmin()
    //{
    //  // Arrange
    //  _testUser = _user;

    //  // Act
    //  HttpResponseMessage response = await _httpClient.GetAsync("values/admin");

    //  // Assert
    //  Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    //}

    // The test is passed no matter the user is admin or not.
    [Fact]
    public async Task GetAdminUser_With_Admin()
    {
      // Arrange
      _testUser = _admin;

      // Act
      HttpResponseMessage response = await _httpClient.GetAsync("values/admin");

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
      _testUser = _user;

      // Act
      HttpResponseMessage response = await _httpClient.GetAsync("values/value-provider");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.Equal(FakeValueProvider.Value, await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Anonymous()
    {
      // Arrange
      _testUser = null;

      // Act
      HttpResponseMessage response = await _httpClient.GetAsync("values/anonymous");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}
