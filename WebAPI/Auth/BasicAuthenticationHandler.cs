using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DataAccess.Model;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Service.Users;

namespace WebAPI.Auth
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IUserService _userService;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
            UrlEncoder encoder, ISystemClock clock, IUserService userService) : base(options, logger, encoder, clock)
        {
            _userService = userService;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return AuthenticateResult.Fail("MissingAuthorizationHeader");
            User user = null;
            try
            {
                AuthenticationHeaderValue authHeader =
                    AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                if (authHeader.Scheme.ToLower().Equals("basic"))
                {
                    byte[] creditentialBytes = Convert.FromBase64String(authHeader.Parameter);
                    string[] creditentials = Encoding.UTF8.GetString(creditentialBytes).Split(':');
                    user = await _userService.Authenticate(creditentials[0], creditentials[1]);
                }

                else return AuthenticateResult.Fail("InvalidAuthorizationHeader");
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail("InvalidAuthorizationHeader");
            }

            if (user == null)
                return AuthenticateResult.Fail("InvalidUsernameorPassword");
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.Role, user.Level.ToString())
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);
            return AuthenticateResult.Success(ticket);
        }
    }
}