namespace Conduit.Api.Authentication;

public static class HttpRequestExtensions
{
    public const string RefreshTokenCookieName = "refreshToken";

    public static string GetRefreshToken(this HttpRequest request)
    {
        if (!TryGetRefreshToken(request, out var token))
            throw new UnauthorizedAccessException("Refresh token não encontrado");

        return token;
    }

    public static bool TryGetRefreshToken(this HttpRequest request, out string token)
    {
        return request.Cookies.TryGetValue(RefreshTokenCookieName, out token!)
            && !string.IsNullOrWhiteSpace(token);
    }
}
