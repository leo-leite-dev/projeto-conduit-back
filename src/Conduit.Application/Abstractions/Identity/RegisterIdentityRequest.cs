namespace Conduit.Application.Abstractions.Identity;

public sealed record RegisterIdentityRequest(string Username, string Email, string Password);
