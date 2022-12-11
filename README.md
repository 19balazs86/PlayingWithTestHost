# Playing with TestHost

This .NET WebAPI is an example of using the built-in `TestServer` and `WebApplicationFactory` **to write integration tests** against your HTTP endpoints.

Authentication can causes unauthorized response in the integration test. The following solutions can be used.

[Separate branch](https://github.com/19balazs86/PlayingWithTestHost/tree/netcoreapp2.2) with the .NET Core 2.2 version.

#### Resources
- [Integration tests in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/test/integration-tests) *(Microsoft Docs)*
- [YouTube link](https://www.youtube.com/watch?v=O3AvN2Rr1uI) *(Microsoft video)*
- Using TestServer, articles: [Link #1](https://koukia.ca/integration-testing-in-asp-net-core-2-0-51d14ede3968) | [Link #2](https://www.infoq.com/articles/testing-aspnet-core-web-api)
- [Use cookie authentication without ASP.NET Core Identity](https://docs.microsoft.com/en-ie/aspnet/core/security/authentication/cookie) *(Microsoft Docs)*
- [Converting integration tests to .NET Core 3.0](https://andrewlock.net/converting-integration-tests-to-net-core-3) *(Andrew Lock)
- [Identity user accounts in integration tests](https://gunnarpeipman.com/testing/aspnet-core-identity-integration-tests/) using `ActionFilter` *(Gunnar Peipman)*

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

- Using `WebApplicationFactory` and override the `ConfigureWebHost` method.
- Using the same authentication mechanism which is defined in the `Startup` file.
- After .NET Core 3, there is an [AuthorizationMiddleware](https://github.com/aspnet/AspNetCore/blob/master/src/Security/Authorization/Policy/src/AuthorizationMiddleware.cs) using a [PolicyEvaluator](https://github.com/aspnet/AspNetCore/blob/master/src/Security/Authorization/Policy/src/PolicyEvaluator.cs) which makes this solution a bit different compared with the previous version.
- Cons: `Authorize` attribute in the controller does not have any effects for the response.

```csharp
protected override void ConfigureWebHost(IWebHostBuilder builder)
{
    builder.ConfigureTestServices(services =>
    {
        services
            .AddSingleton<IValueProvider, FakeValueProvider>()
            .AddSingleton<IPolicyEvaluator>(_ => new FakeUserPolicyEvaluator(() => TestUser?.ToClaims()));
    });
}
```

#### Solution #3

- Using `WebApplicationFactory` and override the `ConfigureWebHost` method.
- Using the `Startup` file, but override the authentication mechanism with the custom `AuthenticationHandler` from the Solution #1.
- `Authorize` attribute effects the response (200, 401, 403).

```csharp
protected override void ConfigureWebHost(IWebHostBuilder builder)
{
    builder.ConfigureTestServices(services =>
    {
        services.AddSingleton<IValueProvider, FakeValueProvider>();

        services.AddTestAuthentication(o => o.TestUserClaimsFunc = () => TestUser?.ToClaims());
    });
}
```

#### Solution #4

- Another way to bypassing the JWT authentication process in my repository: [PlayingWithSignalR](https://github.com/19balazs86/PlayingWithSignalR).
- Apply a custom `DelegatingHandler` in the `CreateDefaultClient` method.
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
