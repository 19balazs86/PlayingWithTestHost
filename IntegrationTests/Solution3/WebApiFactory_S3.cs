﻿using IntegrationTests.Solution1.Dummy;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PlayingWithTestHost;
using PlayingWithTestHost.Model;

namespace IntegrationTests.Solution3
{
    public class WebApiFactory_S3 : WebApplicationFactory<Startup>
    {
        public UserModel TestUser { get; set; }

        public HttpClient HttpClient { get; private set; }

        public WebApiFactory_S3()
        {
            HttpClient = CreateClient();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                services.RemoveAll<IValueProvider>(); // This is not necessary, just to make sure.
                services.AddSingleton<IValueProvider, FakeValueProvider>();

                services.AddTestAuthentication(configureOptions);
            });
        }

        private void configureOptions(TestAuthenticationOptions options)
        {
            options.TestUserClaimsFunc = () => TestUser?.ToClaims();
        }
    }
}
