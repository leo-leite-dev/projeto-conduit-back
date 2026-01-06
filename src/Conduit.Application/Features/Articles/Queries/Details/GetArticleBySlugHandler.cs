using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.Details;

public sealed class GetArticleBySlugQueryHandler
    : IRequestHandler<GetArticleBySlugQuery, Result<ArticleResult>>
{
    private readonly IArticleRepository _articleRepository;

    public GetArticleBySlugQueryHandler(IArticleRepository articleRepository)
    {
        _articleRepository = articleRepository;
    }

    public async Task<Result<ArticleResult>> Handle(
        GetArticleBySlugQuery query,
        CancellationToken ct
    )
    {
        var article = await _articleRepository.GetBySlugAsync(query.Slug, ct);

        if (article is null)
            return Result<ArticleResult>.Failure(ArticleErrors.NotFound);

        return Result<ArticleResult>.Success(new ArticleResult(article));
    }
}
