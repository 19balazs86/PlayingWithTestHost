using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using PlayingWithTestHost.IntegrationTests.Solution2;
using PlayingWithTestHost.Model;
using Xunit;

namespace PlayingWithTestHost.IntegrationTests
{
  public class ValuesControllerTest_S2 : IntegrationTestBase_S2
  {
    private readonly UserModel _user, _admin;

    public ValuesControllerTest_S2(WebApiFactory_S2 webApiFactory) : base(webApiFactory)
    {
      _user  = new UserModel("Test user", new[] { "User" });
      _admin = new UserModel("Test admin", new[] { "Admin" });
    }

    [Theory]
    [InlineData("values",        typeof(IEnumerable<string>))]
    [InlineData("values/config", typeof(TestConfig))]
    [InlineData("values/user",   typeof(UserModel))]
    public async Task GetValues(string requestUri, Type objectType)
    {
      // Arrange
      _testUser = _user;

      HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), requestUri);

      // Act
      HttpResponseMessage response = await _httpClient.SendAsync(request);
      
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
    //  HttpClient httpClient = createClientFor(_user);

    //  // Act
    //  HttpResponseMessage response = await httpClient.GetAsync("values/admin");

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
      // Act
      _testUser = _user;

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
