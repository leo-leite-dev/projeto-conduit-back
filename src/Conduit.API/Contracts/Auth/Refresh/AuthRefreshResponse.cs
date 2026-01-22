namespace Conduit.Api.Contracts.Auth.Refresh;

public sealed class AuthRefreshResponse
{
    public string AccessToken { get; init; } = null!;
    public DateTime AccessTokenExpiresAt { get; init; }
}
