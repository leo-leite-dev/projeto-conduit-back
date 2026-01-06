using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.Details;

public sealed record GetArticleBySlugQuery(string Slug) : IRequest<Result<ArticleResult>>;
