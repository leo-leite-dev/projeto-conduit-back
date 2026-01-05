namespace Conduit.Api.Contracts.Auth;

public sealed record RegisterUserResponse(UserResponse User);

public sealed record UserResponse(string Id, string Username, string Email);
