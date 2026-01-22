using Conduit.Application.Abstractions.Auth;

namespace Conduit.API.Security;

public sealed class CurrentUser : ICurrentUser
{
    public Guid? UserId { get; }
    public string? Username { get; }
    public string? Email { get; }
    public string? Status { get; }

    public bool IsAuthenticated => UserId.HasValue;

    public CurrentUser(IHttpContextAccessor accessor)
    {
        var context = accessor.HttpContext;

        if (context is null)
            return;

        if (Guid.TryParse(context.Items["UserId"]?.ToString(), out var userId))
            UserId = userId;

        Username = context.Items["Username"]?.ToString();
        Email = context.Items["Email"]?.ToString();
        Status = context.Items["Status"]?.ToString();
    }
}
