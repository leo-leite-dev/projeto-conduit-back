using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Unfavorite;

public sealed record UnfavoriteArticleCommand(string Slug) : IRequest<Result<ArticleResult>>;
