namespace Conduit.Api.Authentication.Contracts.Auth.Login;

public sealed class LoginUserDto
{
    public string Email { get; init; } = default!;
    public string Password { get; init; } = default!;
}
