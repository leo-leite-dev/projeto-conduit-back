namespace Conduit.Api.Authentication.Contracts;

public sealed record GetCurrentUserResponse(Guid Id, string Username, string Email);
