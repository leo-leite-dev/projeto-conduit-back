namespace Conduit.Api.Authentication.Contracts.Auth.Login;

public sealed record AuthLoginResponse(string AccessToken, DateTime AccessTokenExpiresAt);
