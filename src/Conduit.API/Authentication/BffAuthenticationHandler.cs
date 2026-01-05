using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace Conduit.Api.Authentication;

public sealed class BffAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private readonly AuthServiceClient _authServiceClient;

    public BffAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        AuthServiceClient authServiceClient
    )
        : base(options, logger, encoder)
    {
        _authServiceClient = authServiceClient;
    }

    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var authorization = Request.Headers.Authorization.ToString();

        Logger.LogInformation("BFF Auth header recebido: {Auth}", authorization);

        if (string.IsNullOrWhiteSpace(authorization))
            return AuthenticateResult.Fail("Sem token");

        if (!authorization.StartsWith("Token ", StringComparison.OrdinalIgnoreCase))
            return AuthenticateResult.Fail("Formato inválido");

        var token = authorization["Token ".Length..].Trim();

        var user = await _authServiceClient.GetCurrentUserAsync(token);

        if (user is null)
            return AuthenticateResult.Fail("Token inválido");

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Email, user.Email),
        };

        var identity = new ClaimsIdentity(claims, Scheme.Name);
        var principal = new ClaimsPrincipal(identity);

        return AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name));
    }
}
