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
    [InlineData("values")]
    [InlineData("values/config")]
    [InlineData("values/user")]
    public async Task GetValues(string requestUri)
    {
      // Arrange
      _fixture.TestUser = _user;

      HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), requestUri);

      // Act
      HttpResponseMessage response = await _fixture.Client.SendAsync(request);
      
      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      string responseString = await response.Content.ReadAsStringAsync();

      Assert.False(string.IsNullOrWhiteSpace(responseString));
    }

    [Fact]
    public async Task CallGetAdminUser_With_NonAdmin()
    {
      // Arrange
      _fixture.TestUser = _user;

      // Act
      HttpResponseMessage response = await _fixture.Client.GetAsync("values/admin");

      // Assert
      Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
    }

    [Fact]
    public async Task CallGetAdminUser_With_Admin()
    {
      // Arrange
      _fixture.TestUser = _admin;

      // Act
      HttpResponseMessage response = await _fixture.Client.GetAsync("values/admin");

      // Assert
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
  }
}
