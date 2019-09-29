using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace PlayingWithTestHost
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddControllers();

      // --> Setup the authentication.
      ConfigureAuthentication(services);

      //services.AddAuthorization();

      // --> BindTo: use the custom extension.
      services.AddSingleton(Configuration.BindTo<TestConfig>());

      // --> Add IValueProvider and override it in the unit test.
      services.AddSingleton<IValueProvider, ValueProvider>();
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
}
