namespace Conduit.Api.Authentication.Contracts;

public sealed record AuthRegisterResponse(
    Guid UserId,
    string AccessToken,
    DateTime AccessTokenExpiresAt
);
