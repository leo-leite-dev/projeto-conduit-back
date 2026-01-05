using Conduit.Application.Abstractions.Results;
using Conduit.Application.Tags.Queries;
using MediatR;

public sealed record GetTagsQuery() : IRequest<Result<GetTagsResult>>;
