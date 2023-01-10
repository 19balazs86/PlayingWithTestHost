using Alba;
using IntegrationTests.Solution1.Dummy;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;

namespace IntegrationTests.Solution4_Alba
{
    // NOTE: This class is not used. Test authentication is applied in the AlbaHostFixture.

    // Based on the Alba AuthenticationStub: https://jasperfx.github.io/alba/guide/security.html
    public sealed class CustomAuthenticationStub : IAlbaExtension
    {
        private readonly Func<IEnumerable<Claim>> _testUserFunc;

        public CustomAuthenticationStub(Func<IEnumerable<Claim>> testUserClaimsFunc)
        {
            _testUserFunc = testUserClaimsFunc;
        }

        IHostBuilder IAlbaExtension.Configure(IHostBuilder builder)
        {
            return builder.ConfigureServices(services => services.AddTestAuthentication(_testUserFunc));
        }

        void IDisposable.Dispose() { }

        ValueTask IAsyncDisposable.DisposeAsync() => ValueTask.CompletedTask;

        Task IAlbaExtension.Start(IAlbaHost host) => Task.CompletedTask;
    }
}
