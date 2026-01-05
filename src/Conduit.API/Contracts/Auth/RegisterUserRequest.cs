namespace Conduit.Api.Contracts.Auth;

public sealed record RegisterUserRequest(RegisterUserDto User);

public sealed record RegisterUserDto(string Username, string Email, string Password);
