using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace PlayingWithTestHost;

public class Startup
{
    public const string DefaultAuthScheme = CookieAuthenticationDefaults.AuthenticationScheme;

    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // --> Setup the authentication.
        ConfigureAuthentication(services);

        // --> Add: Options and validate
        services.AddOptions<TestConfig>()
            .BindConfiguration(nameof(TestConfig))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // services.AddOptionsWithValidateOnStart<TestConfig>(nameof(TestConfig)).ValidateDataAnnotations();
        // services.AddOptionsWithValidateOnStart<TestConfig, TypeOfValidator>(nameof(TestConfig));

        // --> Add IValueProvider and override it in the unit test.
        services.AddSingleton<IValueProvider, ValueProvider>();

        // --> Configure: Authorization
        // FallbackPolicy, by default, is configured to allow requests without authorization.
        // With the following configuration, requires authentication on all endpoints except when [AllowAnonymous].
        services.AddAuthorizationBuilder() // .AddAuthorization() is applied with this line
            .SetFallbackPolicy(new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build());
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    protected virtual void ConfigureAuthentication(IServiceCollection services)
    {
        services
          .AddAuthentication(DefaultAuthScheme)
          .AddCookie(DefaultAuthScheme, options =>
          {
              //options.LoginPath  = "/user/login";
              //options.LogoutPath = "/user/logout";
              options.Events.OnRedirectToLogin = context =>
              {
                  context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                  return Task.CompletedTask;
              };

              options.Events.OnRedirectToAccessDenied = context =>
              {
                  context.Response.StatusCode = StatusCodes.Status403Forbidden;
                  return Task.CompletedTask;
              };
          });
    }
}