using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Marketing.Api.Security
{
    public class BearerTokenAuthenticationOptions : AuthenticationSchemeOptions
    {
        public List<GlobalBearerToken> GlobalBearerTokens { get; set; } = new();
    }

    public class BearerTokenAuthenticationHandler : AuthenticationHandler<BearerTokenAuthenticationOptions>
    {
        public BearerTokenAuthenticationHandler(IOptionsMonitor<BearerTokenAuthenticationOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock) 
            : base(options, logger, encoder, clock)
        {
        }
        
        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            // Check for Authorization header
            if (!Request.Headers.ContainsKey("Authorization")) return Task.FromResult(AuthenticateResult.NoResult());

            // Check for bearer token
            var header = Request.Headers["Authorization"];
            var value = header.ToString().Trim();
            if (!value.StartsWith("bearer ", StringComparison.OrdinalIgnoreCase)) return Task.FromResult(AuthenticateResult.NoResult());

            // Validate token
            var token = value[7..];
            var globalToken = Options.GlobalBearerTokens.FirstOrDefault(g => g.Token.Equals(token, StringComparison.Ordinal));
            if (globalToken == default) return Task.FromResult(AuthenticateResult.NoResult());

            // Ensure token hasn't expired
            if (globalToken.Expiration < DateTime.Today) return Task.FromResult(AuthenticateResult.Fail("Unauthorized"));

            // Build authentication ticket
            var claims = new List<Claim>
            {
                new(ClaimTypes.Name, globalToken.Name),
                new(ClaimTypes.Sid, token)
            };
            var identity = new ClaimsIdentity(claims, "Bearer");
            var principal = new GenericPrincipal(identity, null);
            var ticket = new AuthenticationTicket(principal, "Bearer");

            // Successful authorization
            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
