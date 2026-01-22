using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Api.Authentication;

public sealed class BffAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    private const string TokenPrefix = "Bearer ";

    private readonly IConfiguration _configuration;

    public BffAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration
    )
        : base(options, logger, encoder)
    {
        _configuration = configuration;
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var path = Request.Path.Value;

        if (
            path is not null
            && (
                path.StartsWith("/users/login")
                || path.Equals("/users")
                || path.StartsWith("/api/auth/login")
                || path.StartsWith("/api/auth/refresh")
            )
        )
        {
            return Task.FromResult(AuthenticateResult.NoResult());
        }

        if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeader))
        {
            Logger.LogWarning("🔐 Header Authorization ausente em rota protegida.");
            return Task.FromResult(AuthenticateResult.Fail("Authorization header ausente"));
        }

        var authorization = authorizationHeader.ToString();

        if (!authorization.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
        {
            Logger.LogWarning("❌ Prefixo inválido no Authorization header.");
            return Task.FromResult(AuthenticateResult.Fail("Formato de token inválido"));
        }

        var token = authorization["Bearer ".Length..].Trim();

        if (string.IsNullOrWhiteSpace(token))
        {
            Logger.LogWarning("❌ Token vazio após remover Bearer.");
            return Task.FromResult(AuthenticateResult.Fail("Token vazio"));
        }

        try
        {
            var secret =
                _configuration["Jwt:SecretKey"]
                ?? throw new InvalidOperationException("Jwt:SecretKey não configurado");

            var key = Encoding.UTF8.GetBytes(secret);

            var parameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ClockSkew = TimeSpan.Zero,
            };

            var principal = new JwtSecurityTokenHandler().ValidateToken(token, parameters, out _);

            if (!principal.HasClaim(c => c.Type == ClaimTypes.Name))
            {
                Logger.LogWarning("❌ Claim Name não encontrada no token.");
                return Task.FromResult(AuthenticateResult.Fail("Claim obrigatória ausente"));
            }

            return Task.FromResult(
                AuthenticateResult.Success(new AuthenticationTicket(principal, Scheme.Name))
            );
        }
        catch (SecurityTokenExpiredException)
        {
            Logger.LogWarning("⏰ Token expirado.");
            return Task.FromResult(AuthenticateResult.Fail("Token expirado"));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "💥 Falha ao validar JWT.");
            return Task.FromResult(AuthenticateResult.Fail("Token inválido"));
        }
    }
}
