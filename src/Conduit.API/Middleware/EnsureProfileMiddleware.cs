using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Domain.Entities;

public sealed class EnsureProfileMiddleware
{
    private readonly RequestDelegate _next;

    public EnsureProfileMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(
        HttpContext context,
        ICurrentUser currentUser,
        IProfileRepository profileRepository
    )
    {
        if (currentUser.IsAuthenticated)
        {
            var profile = await profileRepository.GetByUsernameAsync(currentUser.Username);

            if (profile is null)
                await profileRepository.AddAsync(Profile.Create(currentUser.Username));
        }

        await _next(context);
    }
}
