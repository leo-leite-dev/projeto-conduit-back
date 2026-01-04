namespace Conduit.Application.Abstractions.Auth;

public sealed record AuthRegisterCommand(
    string Username,
    string Email,
    string Password
);
