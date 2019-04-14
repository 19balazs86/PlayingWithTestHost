using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace PlayingWithTestHost
{
  public class Startup
  {
    public IConfiguration Configuration { get; }

    public Startup(IConfiguration configuration) => Configuration = configuration;

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

      // --> Setup the authentication.
      ConfigureAuthentication(services);

      // --> BindTo: use the custom extension.
      services.AddSingleton(Configuration.BindTo<TestConfig>());

      // --> Add IValueProvider and override it in the unit test.
      services.AddSingleton<IValueProvider, ValueProvider>();
    }

    public void Configure(IApplicationBuilder app, IHostingEnvironment env)
    {
      if (env.IsDevelopment())
        app.UseDeveloperExceptionPage();

      app.UseAuthentication();

      app.UseMvc();
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
        });
    }
  }
}
