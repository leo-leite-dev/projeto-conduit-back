using Conduit.Domain.Entities;

namespace Conduit.Application.Features.Articles.Results;

public sealed record ArticlesResult(IReadOnlyList<Article> Articles, int Total);
