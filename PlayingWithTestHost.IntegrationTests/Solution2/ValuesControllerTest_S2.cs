using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using PlayingWithTestHost.IntegrationTests.Solution2;
using PlayingWithTestHost.Model;
using Xunit;

namespace PlayingWithTestHost.IntegrationTests
{
  public class ValuesControllerTest_S2 : IntegrationTestBase_S2
  {
    private readonly UserModel _user, _admin;
    private readonly HttpClient _httpClient;

    public ValuesControllerTest_S2(WebApplicationFactory<Startup> factory) : base(factory)
    {
      _user  = new UserModel("Test user", new[] { "User" });
      _admin = new UserModel("Test admin", new[] { "Admin" });

      _httpClient = createClientFor(_user);
    }

    [Theory]
    [InlineData("values",         typeof(IEnumerable<string>))]
    [InlineData("values/config",  typeof(TestConfig))]
    [InlineData("values/user",    typeof(UserModel))]
    public async Task GetValues(string requestUri, Type objectType)
    {
      // Arrange
      HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), requestUri);

      // Act
      HttpResponseMessage response = await _httpClient.SendAsync(request);
      
      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      object responseObject = await response.Content.ReadAsAsync(objectType);

      Assert.NotNull(responseObject);
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
      HttpClient httpClient = createClientFor(_admin);

      // Act
      HttpResponseMessage response = await httpClient.GetAsync("values/admin");

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
      HttpResponseMessage response = await _httpClient.GetAsync("values/value-provider");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
      Assert.Equal(FakeValueProvider.Value, await response.Content.ReadAsStringAsync());
    }

    [Fact]
    public async Task Anonymous()
    {
      // Arrange
      HttpClient httpClient = createClientFor(user: null);

      // Act
      HttpResponseMessage response = await httpClient.GetAsync("values/anonymous");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}
