namespace Conduit.Application.User.Results;

public sealed record GetCurrentUserResponse(string Username, string? Bio, string? Image);
