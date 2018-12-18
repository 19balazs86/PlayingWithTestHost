# Playing with TestHost

This small .NET Core WebAPI application is an example for **using the built-in TestServer** in the Microsoft.AspNetCore.TestHost assembly **to write integration tests**.

Usually you use authentication, which can lead you an unauthorized response in the integration test. This example gives you a solution for this issue.

Resources:
- Microsoft video: [YouTube link.](https://www.youtube.com/watch?v=O3AvN2Rr1uI "YouTube link.")
- Medium article: [Integration Testing in Asp.Net Core.](https://koukia.ca/integration-testing-in-asp-net-core-2-0-51d14ede3968 "Integration Testing in Asp.Net Core.")
- Medium article: [Bypassing ASP.NET Core Authorize in integration tests.](https://medium.com/jackwild/bypassing-asp-net-core-2-0-authorize-tags-in-integration-tests-7bda8fcb0eca "Bypassing ASP.NET Core Authorize in integration tests.")
- Microsoft official page: [Use cookie authentication without ASP.NET Core Identity.](https://docs.microsoft.com/en-ie/aspnet/core/security/authentication/cookie?view=aspnetcore-2.2 "Use cookie authentication without ASP.NET Core Identity.")