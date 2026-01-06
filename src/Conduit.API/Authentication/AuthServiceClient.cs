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
        try
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
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de comunicação com AuthService no register");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout ao chamar AuthService no register");
            throw;
        }
    }

    public async Task<AuthLoginResponse> LoginAsync(LoginUserRequest request, CancellationToken ct)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(
                "/api/auth/login",
                new { login = request.User.Email, password = request.User.Password },
                ct
            );

            if (!response.IsSuccessStatusCode)
                throw await CreateException(response, ct);

            return await response.Content.ReadFromJsonAsync<AuthLoginResponse>(ct)
                ?? throw new InvalidOperationException("Resposta inválida do AuthService.");
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de comunicação com AuthService no login");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout ao chamar AuthService no login");
            throw;
        }
    }

    public async Task<GetCurrentUserResponse?> GetCurrentUserAsync(
        string token,
        CancellationToken ct = default
    )
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(
                "Bearer",
                token
            );

            var response = await _httpClient.GetAsync("/api/users/me", ct);

            if (!response.IsSuccessStatusCode)
                return null;

            return await response.Content.ReadFromJsonAsync<GetCurrentUserResponse>(ct);
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de comunicação com AuthService ao validar token");
            throw;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout ao validar token no AuthService");
            throw;
        }
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
