namespace Conduit.Api.Authentication.Contracts.Auth.Register;

public sealed record AuthRegisterResponse(
    Guid UserId,
    string AccessToken,
    DateTime AccessTokenExpiresAt
);
