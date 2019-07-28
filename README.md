# Playing with TestHost

This .NET Core WebAPI application is an example for using the built-in `TestServer` and `WebApplicationFactory` **to write integration tests** against your HTTP endpoints.

Authentication can causes unauthorized response in the integration test. I prepared 3+1 types of solutions for this issue.

[Separate branch](https://github.com/19balazs86/PlayingWithTestHost/tree/netcoreapp2.2) with the .NET Core 2.2 version.

#### Resources
- Microsoft Docs: [Integration tests in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests?view=aspnetcore-3.0).
- Microsoft video: [YouTube link.](https://www.youtube.com/watch?v=O3AvN2Rr1uI)
- Medium article: [Integration Testing in Asp.Net Core.](https://koukia.ca/integration-testing-in-asp-net-core-2-0-51d14ede3968)
- InfoQ article: [How to Test ASP.NET Core Web API.](https://www.infoq.com/articles/testing-aspnet-core-web-api)
- Microsoft Docs: [Use cookie authentication without ASP.NET Core Identity.](https://docs.microsoft.com/en-ie/aspnet/core/security/authentication/cookie?view=aspnetcore-3.0)

##### Authentication solutions in integration test
- Medium article: [Bypassing ASP.NET Core Authorize in integration tests.](https://medium.com/jackwild/bypassing-asp-net-core-2-0-authorize-tags-in-integration-tests-7bda8fcb0eca)
- Gunnar Peipman blog: [Identity user accounts in integration tests](https://gunnarpeipman.com/testing/aspnet-core-identity-integration-tests/) (`ActionFilter`).

#### Solution #1

- Using `WebHostBuilder` to create a `TestServer` 'manually'.
- Using a `TestStartup` class derived from `Startup`.
- Apply a custom `AuthenticationHandler` to authorize the request.
- `Authorize` attribute affects the response (200, 401, 403).

```csharp
IWebHostBuilder builder = new WebHostBuilder()
    .UseStartup<TestStartup>();

_testServer = new TestServer(builder);

Client = _testServer.CreateClient();
```

#### Solution #2

- Using `WebApplicationFactory` and override the `CreateWebHostBuilder` method.
- Using the same authentication mechanism which is defined in the `Startup` file.
- After .NET Core 3, there is an [AuthorizationMiddleware](https://github.com/aspnet/AspNetCore/blob/master/src/Security/Authorization/Policy/src/AuthorizationMiddleware.cs) using a [PolicyEvaluator](https://github.com/aspnet/AspNetCore/blob/master/src/Security/Authorization/Policy/src/PolicyEvaluator.cs) which makes this solution a bit different compared with the previous version.
- Cons: `Authorize` attribute in the controller does not have any effects for the response.

```csharp
protected override IHostBuilder CreateHostBuilder()
{
  return Host
    .CreateDefaultBuilder()
    .ConfigureWebHostDefaults(webHostBuilder =>
      webHostBuilder
        .UseStartup<Startup>()
        .ConfigureTestServices(services =>
            services.AddSingleton<IPolicyEvaluator>(_ => new FakeUserPolicyEvaluator(...))));
}
```

#### Solution #3

- Using `WebApplicationFactory` and override the `CreateWebHostBuilder` method.
- Using the `Startup` file, but override the authentication mechanism with the custom `AuthenticationHandler` from the Solution #1.
- `Authorize` attribute effects the response (200, 401, 403).

```csharp
protected override IWebHostBuilder CreateWebHostBuilder()
{
    return WebHost
        .CreateDefaultBuilder()
        .ConfigureTestServices(services =>
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = TestStartup.AuthScheme;
                options.DefaultChallengeScheme    = TestStartup.AuthScheme;
            })
            .AddTestAuth(o => o.TestUserClaimsFunc = () => TestUser?.ToClaims());
        })
        .UseStartup<Startup>();
}
```

#### Solution #4

- There is another way to bypassing the authorize process in my repository: [PlayingWithSignalR](https://github.com/19balazs86/PlayingWithSignalR).
- Using a `DelegatingHandler` and pass it to the `CreateDefaultClient` method.
- The handler injects a token in the `Authorization` header.

```csharp
public class WebApiFactory : WebApplicationFactory<Startup>
{
    public WebApiFactory()
    {
        HttpClient = CreateDefaultClient(new AuthDelegatingHandler(...));
    }
}
```
```csharp
public class AuthDelegatingHandler : DelegatingHandler
{
  protected override Task<HttpResponseMessage> SendAsync(...)
  {
    request.Headers.Authorization = ...;

    return base.SendAsync(request, cancelToken);
  }
}
```
