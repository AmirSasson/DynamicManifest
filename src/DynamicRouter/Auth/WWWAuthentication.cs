using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace DynamicRoutes.Auth
{
    public class WWWAuthenticationHandler : AuthenticationHandler<WWWAuthenticationOptions>
    {
        public WWWAuthenticationHandler(IOptionsMonitor<WWWAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        {

        }
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {            
            var cer = base.Context.Connection.ClientCertificate;
            return AuthenticateResult.Success(CreateTicket()).AsTask();
            //return AuthenticateResult.Fail(new Exception("Authentication Failure")).AsTask();
        }

        private AuthenticationTicket CreateTicket()
        {
            var claims = new List<Claim> { };
            var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, nameof(WWWAuthenticationHandler)));
            return new AuthenticationTicket(principal, nameof(WWWAuthenticationHandler));
        }
    }

    public class WWWAuthenticationOptions : AuthenticationSchemeOptions
    {
        public bool SkipClientCertificateChecks { get; set; }
    }
}
