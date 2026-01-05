using Conduit.Domain.Entities;

namespace Conduit.Application.Features.Articles.Queries;

public sealed record ArticlesResult(IReadOnlyList<Article> Articles, int Total);
