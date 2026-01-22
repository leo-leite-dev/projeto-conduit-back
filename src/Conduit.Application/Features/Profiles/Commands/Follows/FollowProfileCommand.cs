using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Profiles.Results;
using MediatR;

namespace Conduit.Application.Features.Profiles.Commands.Follows;

public sealed record FollowProfileCommand(string Username) : IRequest<Result<GetProfileResponse>>;
