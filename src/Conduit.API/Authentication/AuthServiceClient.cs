using Conduit.Api.Authentication.Contracts;
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

    public async Task<AuthRegisterResponse> RegisterAsync(
        RegisterUserRequest request,
        CancellationToken ct
    )
    {
        var httpResponse = await _httpClient.PostAsJsonAsync(
            "/api/auth/register",
            new
            {
                username = request.User.Username,
                email = request.User.Email,
                password = request.User.Password,
            },
            ct
        );

        if (!httpResponse.IsSuccessStatusCode)
            throw await CreateException(httpResponse, ct);

        var authResponse =
            await httpResponse.Content.ReadFromJsonAsync<AuthRegisterResponse>(ct)
            ?? throw new InvalidOperationException("Resposta inválida do AuthService.");

        return authResponse;
    }

    private static async Task<BffHttpException> CreateException(
        HttpResponseMessage response,
        CancellationToken ct
    )
    {
        ApiErrorResponse? error = null;

        if (
            response.Content.Headers.ContentLength > 0
            && response.Content.Headers.ContentType?.MediaType == "application/json"
        )
            error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(ct);

        return new BffHttpException(
            response.StatusCode,
            error?.Type ?? "auth_service_error",
            error?.Message ?? "Erro ao comunicar com AuthService"
        );
    }
}
