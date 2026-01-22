namespace Conduit.Application.Features.Profiles.Results;

public sealed class ProfileResponse
{
    public string Username { get; init; } = null!;
    public string? Bio { get; init; }
    public string? Image { get; init; }
    public bool Following { get; init; }
}
