using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace IntegrationTests.Solution1.Dummy;

public static class TestAuthenticationExtensions
{
    public static IServiceCollection AddTestAuthentication(this IServiceCollection services, Func<IEnumerable<Claim>> testUserClaimsFunc)
    {
        return services.AddTestAuthentication(options => options.TestUserClaimsFunc = testUserClaimsFunc);
    }

    public static IServiceCollection AddTestAuthentication(this IServiceCollection services, Action<TestAuthenticationOptions> configureOptions)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = Consts.AuthScheme;
            options.DefaultChallengeScheme    = Consts.AuthScheme;
        })
        .addTestAuthenticationScheme(configureOptions);

        return services;
    }

    private static AuthenticationBuilder addTestAuthenticationScheme(this AuthenticationBuilder builder, Action<TestAuthenticationOptions> configureOptions)
    {
        return builder.AddScheme<TestAuthenticationOptions, TestAuthenticationHandler>(Consts.AuthScheme, configureOptions);
    }
}

public class TestAuthenticationHandler : AuthenticationHandler<TestAuthenticationOptions>
{
    public TestAuthenticationHandler(
        IOptionsMonitor<TestAuthenticationOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        ClaimsIdentity claimsIdentity = Options.Identity();

        if (claimsIdentity is null)
            return Task.FromResult(AuthenticateResult.NoResult());

        var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Consts.AuthScheme);

        return Task.FromResult(AuthenticateResult.Success(authenticationTicket));
    }
}

public class TestAuthenticationOptions : AuthenticationSchemeOptions
{
    public Func<IEnumerable<Claim>> TestUserClaimsFunc { get; set; }

    public ClaimsIdentity Identity()
    {
        IEnumerable<Claim> claims = TestUserClaimsFunc?.Invoke();

        if (claims is null) return null;

        return new ClaimsIdentity(claims, Consts.AuthScheme);
    }
}

file class Consts
{
    public const string AuthScheme = "TestAuthScheme";
}
