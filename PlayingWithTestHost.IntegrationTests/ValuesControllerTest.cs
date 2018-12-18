using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace PlayingWithTestHost.IntegrationTests
{
  public class ValuesControllerTest : IClassFixture<TestServerFixture>
  {
    private readonly TestServerFixture _fixture;

    public ValuesControllerTest(TestServerFixture fixture)
    {
      _fixture = fixture;
    }

    [Theory]
    [InlineData("values")]
    [InlineData("values/config")]
    [InlineData("values/user")]
    public async Task GetValues(string requestUri)
    {
      // Arrange
      HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), requestUri);

      // Act
      HttpResponseMessage response = await _fixture.Client.SendAsync(request);

      Exception ensureException = Record.Exception(() => response.EnsureSuccessStatusCode());
      
      // Assert
      Assert.Null(ensureException);
      Assert.Equal(HttpStatusCode.OK, response.StatusCode);

      string responseString = await response.Content.ReadAsStringAsync();

      Assert.False(string.IsNullOrWhiteSpace(responseString));
    }
  }
}
