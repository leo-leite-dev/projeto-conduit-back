namespace Conduit.Api.Authentication.Contracts;

public sealed record AuthLoginResponse(string AccessToken, DateTime AccessTokenExpiresAt);
