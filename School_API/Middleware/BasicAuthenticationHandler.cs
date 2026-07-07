using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Text;

namespace School_API.Middleware
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return Task.FromResult(AuthenticateResult.NoResult());

            try
            {
                var authHeaderValue = Request.Headers["Authorization"].ToString();
                if (string.IsNullOrEmpty(authHeaderValue))
                    return Task.FromResult(AuthenticateResult.NoResult());

                var authHeader = AuthenticationHeaderValue.Parse(authHeaderValue);
                if (authHeader.Scheme != "Basic")
                    return Task.FromResult(AuthenticateResult.NoResult());

                var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter ?? string.Empty)).Split(':');
                if (credentials.Length != 2)
                    return Task.FromResult(AuthenticateResult.Fail("Invalid credentials format"));

                var username = credentials[0];
                var password = credentials[1];

                var basicAuthUsername = "B2BUser";
                var basicAuthPassword = "B2B@2025!Secure";

                if (username == basicAuthUsername && password == basicAuthPassword)
                {
                    var claims = new[] 
                    { 
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.AuthenticationMethod, "Basic")
                    };
                    var identity = new ClaimsIdentity(claims, "Basic");
                    var principal = new ClaimsPrincipal(identity);
                    var ticket = new AuthenticationTicket(principal, "Basic");
                    return Task.FromResult(AuthenticateResult.Success(ticket));
                }

                return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail($"Authentication error: {ex.Message}"));
            }
        }
    }
}

