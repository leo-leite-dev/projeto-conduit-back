using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Favorite;

public sealed record FavoriteArticleCommand(string Slug) : IRequest<Result<ArticleResult>>;
