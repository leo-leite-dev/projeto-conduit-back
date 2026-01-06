using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.Comments;

public sealed record GetArticleCommentsQuery(string Slug) : IRequest<Result<ArticleCommentsResult>>;
