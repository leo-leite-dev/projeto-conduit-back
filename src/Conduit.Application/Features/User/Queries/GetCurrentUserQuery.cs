using Conduit.Application.Abstractions.Results;
using Conduit.Application.User.Results;
using MediatR;

public sealed record GetCurrentUserQuery : IRequest<Result<GetCurrentUserResponse>>;
