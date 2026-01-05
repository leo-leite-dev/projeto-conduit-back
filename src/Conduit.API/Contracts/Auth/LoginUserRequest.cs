namespace Conduit.Api.Contracts.Auth;

public sealed record LoginUserRequest(LoginUserDto User);

public sealed record LoginUserDto(string Email, string Password);
