using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Profiles.Results;
using MediatR;

namespace Conduit.Application.Features.Profiles.Commands.Unfollows;

public sealed record UnfollowProfileCommand(string Username) : IRequest<Result<GetProfileResponse>>;
