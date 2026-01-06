using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.Global;

public sealed record GetArticlesQuery(int Limit, int Offset) : IRequest<Result<ArticlesResult>>;
