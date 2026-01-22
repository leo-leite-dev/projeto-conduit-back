namespace Conduit.Application.Features.Articles.Results;

public sealed record ArticlesResult(IReadOnlyList<ArticleResult> Articles, int ArticlesCount);
