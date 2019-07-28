# Playing with TestHost

This .NET Core WebAPI application is an example for using the built-in `TestServer` and `WebApplicationFactory` **to write integration tests** against your HTTP endpoints.

Authentication can causes unauthorized response in the integration test. I prepared 3 types of solutions for this issue.

#### Resources
- Microsoft Docs: [Integration tests in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-2.2).
- Microsoft video: [YouTube link.](https://www.youtube.com/watch?v=O3AvN2Rr1uI)
- Medium article: [Integration Testing in Asp.Net Core.](https://koukia.ca/integration-testing-in-asp-net-core-2-0-51d14ede3968)
- InfoQ article: [How to Test ASP.NET Core Web API.](https://www.infoq.com/articles/testing-aspnet-core-web-api)
- Microsoft Docs: [Use cookie authentication without ASP.NET Core Identity.](https://docs.microsoft.com/en-ie/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2)

##### Authentication solutions in integration test
- Medium article: [Bypassing ASP.NET Core Authorize in integration tests.](https://medium.com/jackwild/bypassing-asp-net-core-2-0-authorize-tags-in-integration-tests-7bda8fcb0eca)
- Gunnar Peipman blog: [Identity user accounts in integration tests](https://gunnarpeipman.com/testing/aspnet-core-identity-integration-tests/) (`ActionFilter`).

> Regarding this topic, worth to check the [Alba](http://jasperfx.github.io/alba/getting_started) - class library to write integration tests.

#### Solution #1 with WebHostBuilder

Using `WebHostBuilder` to create a `TestServer`. Generate a `HttpClient` to call our WebAPI.

```csharp
IWebHostBuilder builder = new WebHostBuilder()
    .UseStartup<TestStartup>();

_testServer = new TestServer(builder);

Client = _testServer.CreateClient();
```

#### Test method

```csharp
[Theory]
[InlineData("values", typeof(IEnumerable<string>))]
[InlineData("values/config", typeof(TestConfig))]
[InlineData("values/user", typeof(UserModel))]
public async Task GetValues(string requestUri, Type objectType)
{
    // Arrange
    HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("GET"), requestUri);

    // Act
    HttpResponseMessage response = await _fixture.Client.SendAsync(request);

    // Assert
    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    object responseObject = await response.Content.ReadAsAsync(objectType);

    Assert.NotNull(responseObject);
}
```
