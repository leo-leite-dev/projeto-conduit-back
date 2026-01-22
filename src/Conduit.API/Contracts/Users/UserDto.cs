namespace Conduit.Api.Contracts.Users;

public sealed class UserDto
{
    public string Email { get; init; } = default!;
    public string Username { get; init; } = default!;
    public string Token { get; init; } = default!;
}
