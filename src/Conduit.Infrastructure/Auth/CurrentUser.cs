using System.Security.Claims;
using Conduit.Application.Abstractions.Auth;
using Microsoft.AspNetCore.Http;

namespace Conduit.Infrastructure.Auth;

public sealed class CurrentUser : ICurrentUser
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUser(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public bool IsAuthenticated =>
        _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

    public string Username
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;

            if (user is null || !user.Identity!.IsAuthenticated)
                throw new InvalidOperationException("User is not authenticated.");

            return user.FindFirstValue(ClaimTypes.Name)
                ?? throw new InvalidOperationException("Username claim not found.");
        }
    }
}
