using Conduit.Api.Contracts.Auth;
using Conduit.API.Contracts.Errors;
using Conduit.API.Exceptions;

namespace Conduit.Api.Authentication;

public sealed class AuthServiceClient
{
    private readonly HttpClient _httpClient;

    public AuthServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<RegisterUserResponse> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken ct
    )
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                username = request.User.Username,
                email = request.User.Email,
                password = request.User.Password,
            },
            ct
        );

        if (response.IsSuccessStatusCode)
        {
            var authResponse =
                await response.Content.ReadFromJsonAsync<AuthRegisterResponse>(ct)
                ?? throw new InvalidOperationException("Resposta inválida do AuthService.");

            return new RegisterUserResponse(
                new UserResponse(
                    authResponse.UserId.ToString(),
                    request.User.Username,
                    request.User.Email
                )
            );
        }

        ApiErrorResponse? error = null;

        if (
            response.Content.Headers.ContentLength > 0
            && response.Content.Headers.ContentType?.MediaType == "application/json"
        )
            error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(ct);

        throw new BffHttpException(
            response.StatusCode,
            error?.Type ?? "unknown_error",
            error?.Message ?? "Erro ao comunicar com AuthService"
        );
    }
}
