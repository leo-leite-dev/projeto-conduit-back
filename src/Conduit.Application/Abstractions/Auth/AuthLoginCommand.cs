namespace Conduit.Application.Abstractions.Auth;

public sealed record AuthLoginCommand(
    string Email,
    string Password
);