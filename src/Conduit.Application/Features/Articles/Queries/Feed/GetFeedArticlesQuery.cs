using Conduit.Application.Abstractions.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.Feed;

public sealed record GetFeedArticlesQuery(int Limit, int Offset) : IRequest<Result<ArticlesResult>>;
