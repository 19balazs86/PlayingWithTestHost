using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

namespace PlayingWithTestHost;

public class Startup
{
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        // --> Setup the authentication.
        ConfigureAuthentication(services);

        services.AddOptions<TestConfig>()
            .BindConfiguration(nameof(TestConfig))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        // --> Add IValueProvider and override it in the unit test.
        services.AddSingleton<IValueProvider, ValueProvider>();

        services.AddAuthorization(options =>
        {
            // https://docs.microsoft.com/en-ie/aspnet/core/migration/22-to-30?view=aspnetcore-3.0&tabs=visual-studio#authorization
            // FallbackPolicy is initially configured to allow requests without authorization.
            // Override it to always require authentication on all endpoints except when [AllowAnonymous].
            options.FallbackPolicy = new AuthorizationPolicyBuilder()
                .RequireAuthenticatedUser()
                .Build();
        });
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
            app.UseDeveloperExceptionPage();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }

    protected virtual void ConfigureAuthentication(IServiceCollection services)
    {
        services
          .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
          .AddCookie(o => {
              //o.LoginPath  = "/user/login";
              //o.LogoutPath = "/user/logout";
              o.Events.OnRedirectToLogin = context =>
              {
                  context.Response.StatusCode = 401;
                  return Task.CompletedTask;
              };

              o.Events.OnRedirectToAccessDenied = context =>
              {
                  context.Response.StatusCode = 403;
                  return Task.CompletedTask;
              };
          });
    }
}
