namespace Conduit.Api.Authentication.Contracts.Auth.Register;

public sealed class RegisterUserDto
{
    public string Email { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Password { get; init; } = default!;
}
