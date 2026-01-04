using System.Net.Http.Json;
using Conduit.Application.Abstractions.Auth;

namespace Conduit.Infrastructure.Auth;

public sealed class AuthServiceClient : IAuthServiceClient
{
    private readonly HttpClient _http;

    public AuthServiceClient(HttpClient http)
    {
        _http = http;
    }

    public async Task<AuthRegisterResult> RegisterAsync(
        AuthRegisterCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _http.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                username = command.Username,
                email = command.Email,
                password = command.Password,
            },
            cancellationToken
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthRegisterResponse>(
            cancellationToken: cancellationToken
        );

        return new AuthRegisterResult(result!.UserId);
    }

    public async Task<AuthLoginResult> LoginAsync(
        AuthLoginCommand command,
        CancellationToken cancellationToken = default
    )
    {
        var response = await _http.PostAsJsonAsync(
            "/api/auth/login",
            new { login = command.Email, password = command.Password },
            cancellationToken
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<AuthLoginResponse>(
            cancellationToken: cancellationToken
        );

        return new AuthLoginResult(result!.AccessToken);
    }

    private sealed record AuthRegisterResponse(Guid UserId);

    private sealed record AuthLoginResponse(
        string AccessToken,
        DateTimeOffset AccessTokenExpiresAt
    );
}
