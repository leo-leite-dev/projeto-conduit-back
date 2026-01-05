using System.Security.Claims;

namespace Conduit.Api.Security.Authentication;

public static class ClaimsPrincipalExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal user) =>
        Guid.Parse(user.FindFirstValue("sub")!);

    public static string GetUsername(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.Name)!;

    public static string GetEmail(this ClaimsPrincipal user) =>
        user.FindFirstValue(ClaimTypes.Email)!;
}
