using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Conduit.Api.Authentication;

public sealed class BffAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BffAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder
    )
        : base(options, logger, encoder) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();

        if (string.IsNullOrWhiteSpace(authorization))
            return Task.FromResult(AuthenticateResult.Fail("Sem token"));

        if (!authorization.StartsWith("Bearer ") && !authorization.StartsWith("Token "))
            return Task.FromResult(AuthenticateResult.Fail("Formato inválido"));

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, "authenticated-user"),
            new Claim(ClaimTypes.Name, "AuthenticatedUser"),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, Scheme.Name);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}
