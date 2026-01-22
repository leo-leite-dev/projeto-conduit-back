using Conduit.API.Exceptions;

namespace Conduit.Api.Middlewares;

public sealed class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;
    private readonly IWebHostEnvironment _env;

    public ErrorHandlingMiddleware(
        RequestDelegate next,
        ILogger<ErrorHandlingMiddleware> logger,
        IWebHostEnvironment env
    )
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (BffHttpException ex)
        {
            _logger.LogWarning(ex, "Erro BFF: {Type}", ex.Type);

            context.Response.StatusCode = (int)ex.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new { type = ex.Type, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro não tratado");

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "application/json";

            if (_env.IsDevelopment())
            {
                await context.Response.WriteAsJsonAsync(
                    new
                    {
                        type = "internal_error",
                        message = ex.Message,
                        stackTrace = ex.StackTrace,
                    }
                );
            }
            else
            {
                await context.Response.WriteAsJsonAsync(
                    new { type = "internal_error", message = "Erro interno. Tente novamente." }
                );
            }
        }
    }
}
