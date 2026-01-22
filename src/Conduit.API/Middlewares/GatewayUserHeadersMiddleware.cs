namespace Conduit.Api.Middlewares;

public sealed class GatewayUserHeadersMiddleware
{
    private readonly RequestDelegate _next;

    public GatewayUserHeadersMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var userId = context.Request.Headers["X-User-Id"].FirstOrDefault();
        var username = context.Request.Headers["X-Username"].FirstOrDefault();
        var email = context.Request.Headers["X-User-Email"].FirstOrDefault();
        var status = context.Request.Headers["X-User-Status"].FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(userId))
        {
            context.Items["UserId"] = userId;
            context.Items["Username"] = username;
            context.Items["Email"] = email;
            context.Items["Status"] = status;
        }

        await _next(context);
    }
}
