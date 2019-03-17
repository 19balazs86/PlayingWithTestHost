# Playing with TestHost

This small .NET Core WebAPI application is an example for **using the built-in TestServer** in the Microsoft.AspNetCore.TestHost assembly **to write integration tests** against your HTTP endpoints.

Authentication can causes unauthorized response in the integration test. The example gives you a solution for this issue.

#### Resources
- Microsoft video: [YouTube link.](https://www.youtube.com/watch?v=O3AvN2Rr1uI "YouTube link")
- Medium article: [Integration Testing in Asp.Net Core.](https://koukia.ca/integration-testing-in-asp-net-core-2-0-51d14ede3968 "Integration Testing in Asp.Net Core")
- InfoQ article: [How to Test ASP.NET Core Web API.](https://www.infoq.com/articles/testing-aspnet-core-web-api "How to Test ASP.NET Core Web API")
- Medium article: [Bypassing ASP.NET Core Authorize in integration tests.](https://medium.com/jackwild/bypassing-asp-net-core-2-0-authorize-tags-in-integration-tests-7bda8fcb0eca "Bypassing ASP.NET Core Authorize in integration tests")
- Microsoft official page: [Use cookie authentication without ASP.NET Core Identity.](https://docs.microsoft.com/en-ie/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2 "Use cookie authentication without ASP.NET Core Identity")

> Regarding the topic, worth to check the [Alba](http://jasperfx.github.io/alba/getting_started "Alba") - class library to write integration tests.

#### WebHostBuilder

WebHostBuilder to create a TestServer, which can genereate the HttpClient to call your API.

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

    // Content.Deserialize: Custom extension for HttpContent.
    // The response can be quite big. Deserialize the response directly from stream
    // to avoid allocating more memory, than necessary.
    object responseObject = await response.Content.Deserialize(objectType);

    Assert.NotNull(responseObject);
}
```
