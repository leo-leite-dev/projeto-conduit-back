using Conduit.Domain.Entities;

namespace Conduit.Application.Features.Articles.Results;

public sealed record ArticleCommentsResult(IReadOnlyList<Comment> Comments);
