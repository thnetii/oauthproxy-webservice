using System;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace THNETII.WebServices.OAuthProxyWebApp.Services
{
    public class BearerSessionAuthHandler : AuthenticationHandler<BearerSessionAuthOptions>
    {
        public BearerSessionAuthHandler(
            IOptionsMonitor<BearerSessionAuthOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeaderValues))
                return Task.FromResult(AuthenticateResult.NoResult());

            const string BearerAuthScheme = "Bearer";
            var (dataProtectionProvider, sessionStore) =
                (Options.DataProtectionProvider, Options.SessionStore);
            var dataProtector = dataProtectionProvider.CreateProtector(
                new[] { GetType().FullName!, Scheme.Name, BearerAuthScheme }
                );

            foreach (string authHeaderString in authHeaderValues)
            {
                if (!AuthenticationHeaderValue.TryParse(authHeaderString, out var authHeader))
                    continue;
                if (!authHeader.Scheme.Equals(BearerAuthScheme, StringComparison.OrdinalIgnoreCase))
                    continue;
                if (!(authHeader.Parameter is { Length: > 0 } accessToken))
                    continue;


                string sessionKey;
                try
                {
                    sessionKey = dataProtector.Unprotect(accessToken);
                }
                catch (CryptographicException except)
                {
                    return Task.FromResult(AuthenticateResult.Fail(except));
                }

                if (!sessionStore.TryGetValue(sessionKey, out var sessionState) || sessionState is null)
                    return Task.FromResult(AuthenticateResult.Fail("Access token refers to a non-existant session. The session may have expired."));
            }

            return Task.FromResult(AuthenticateResult.NoResult());
        }
    }

    public class BearerSessionAuthOptions : AuthenticationSchemeOptions
    {
        public IMemoryCache SessionStore { get; set; } = default!;
        public IDataProtectionProvider DataProtectionProvider { get; set; } = default!;
    }

    public class BearerSessionAuthPostConfigureOptions
        : IPostConfigureOptions<BearerSessionAuthOptions>
    {
        private readonly IDataProtectionProvider dataProtectionProvider;

        public BearerSessionAuthPostConfigureOptions(
            IDataProtectionProvider dataProtectionProvider)
        {
            this.dataProtectionProvider = dataProtectionProvider
                ?? throw new ArgumentNullException(nameof(dataProtectionProvider));
        }

        public void PostConfigure(string name, BearerSessionAuthOptions options)
        {
            _ = options ?? throw new ArgumentNullException(nameof(options));

            options.DataProtectionProvider ??= dataProtectionProvider;
        }
    }
}
