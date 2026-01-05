using System.Net.Http.Headers;
using Conduit.Api.Authentication.Contracts;
using Conduit.Api.Contracts.Auth;
using Conduit.API.Contracts.Errors;
using Conduit.API.Exceptions;

namespace Conduit.Api.Authentication;

public sealed class AuthServiceClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AuthServiceClient> _logger;

    public AuthServiceClient(HttpClient httpClient, ILogger<AuthServiceClient> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<AuthRegisterResponse> RegisterAsync(
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

        if (!response.IsSuccessStatusCode)
            throw await CreateException(response, ct);

        return await response.Content.ReadFromJsonAsync<AuthRegisterResponse>(ct)
            ?? throw new InvalidOperationException("Resposta inválida do AuthService.");
    }

    public async Task<GetCurrentUserResponse?> GetCurrentUserAsync(
        string token,
        CancellationToken ct = default
    )
    {
        _logger.LogInformation("AuthServiceClient: Validating token");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
            "Bearer",
            token
        );

        var response = await _httpClient.GetAsync("/api/users/me", ct);

        _logger.LogInformation(
            "AuthServiceClient: Response status {StatusCode}",
            response.StatusCode
        );

        if (!response.IsSuccessStatusCode)
            return null;

        return await response.Content.ReadFromJsonAsync<GetCurrentUserResponse>(ct);
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
