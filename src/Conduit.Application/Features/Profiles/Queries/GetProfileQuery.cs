using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Profiles.Results;
using MediatR;

namespace Conduit.Application.Features.Profiles.Queries;

public sealed record GetProfileQuery(string Username) : IRequest<Result<GetProfileResponse>>;
