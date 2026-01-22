using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Profiles.Results;
using MediatR;

public sealed record CreateProfileCommand(string Username, string? Bio, string? Image)
    : IRequest<Result<GetProfileResponse>>;
