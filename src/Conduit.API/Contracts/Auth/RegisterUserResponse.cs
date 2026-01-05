namespace Conduit.Api.Contracts.Auth;

public sealed record RegisterUserResponse(UserResponse User);

public sealed record UserResponse(string Email, string Username, string Token);
