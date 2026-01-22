using System.Net.Http.Headers;
using Conduit.Api.Authentication.Contracts.Auth.Login;
using Conduit.Api.Authentication.Contracts.Auth.Register;
using Conduit.Api.Contracts.Auth.Refresh;
using Conduit.API.Contracts.Errors;
using Conduit.API.Exceptions;
using Conduit.Application.User.Results;

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
        return await SendAsync<AuthRegisterResponse>(
            () =>
                _httpClient.PostAsJsonAsync(
                    "/api/auth/register",
                    new
                    {
                        username = request.User.Username,
                        email = request.User.Email,
                        password = request.User.Password,
                    },
                    ct
                ),
            "register",
            ct
        );
    }

    public async Task<AuthLoginResponse> LoginAsync(LoginUserRequest request, CancellationToken ct)
    {
        return await SendAsync<AuthLoginResponse>(
            () =>
                _httpClient.PostAsJsonAsync(
                    "/api/auth/login",
                    new { login = request.User.Email, password = request.User.Password },
                    ct
                ),
            "login",
            ct
        );
    }

    public async Task<GetCurrentUserResponse?> GetCurrentUserAsync(
        string token,
        CancellationToken ct = default
    )
    {
        try
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, "/api/users/me");

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request, ct);

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

    private async Task<T> SendAsync<T>(
        Func<Task<HttpResponseMessage>> action,
        string operation,
        CancellationToken ct
    )
    {
        try
        {
            var response = await action();

            if (!response.IsSuccessStatusCode)
                throw await CreateException(response, ct);

            return await response.Content.ReadFromJsonAsync<T>(ct)
                ?? throw new InvalidOperationException(
                    $"Resposta inválida do AuthService ({operation})."
                );
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Erro de comunicação com AuthService no {Operation}", operation);
            throw;
        }
        catch (TaskCanceledException ex)
        {
            _logger.LogError(ex, "Timeout ao chamar AuthService no {Operation}", operation);
            throw;
        }
    }

    public async Task<AuthRefreshResponse> RefreshAsync(string refreshToken, CancellationToken ct)
    {
        var response = await _httpClient.PostAsJsonAsync(
            "/api/auth/refresh",
            new { refreshToken },
            ct
        );

        if (!response.IsSuccessStatusCode)
            throw await CreateException(response, ct);

        return await response.Content.ReadFromJsonAsync<AuthRefreshResponse>(ct)
            ?? throw new InvalidOperationException("Resposta inválida do AuthService");
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
        {
            error = await response.Content.ReadFromJsonAsync<ApiErrorResponse>(ct);
        }

        return new BffHttpException(
            response.StatusCode,
            error?.Type ?? "auth_service_error",
            error?.Message ?? "Erro ao comunicar com AuthService"
        );
    }
}
