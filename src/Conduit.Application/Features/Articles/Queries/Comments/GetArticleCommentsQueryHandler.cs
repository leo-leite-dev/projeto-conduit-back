using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Errors;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.Comments;

public sealed class GetArticleCommentsQueryHandler
    : IRequestHandler<GetArticleCommentsQuery, Result<ArticleCommentsResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ICommentRepository _commentRepository;

    public GetArticleCommentsQueryHandler(
        IArticleRepository articleRepository,
        ICommentRepository commentRepository
    )
    {
        _articleRepository = articleRepository;
        _commentRepository = commentRepository;
    }

    public async Task<Result<ArticleCommentsResult>> Handle(
        GetArticleCommentsQuery query,
        CancellationToken ct
    )
    {
        var article = await _articleRepository.GetBySlugAsync(query.Slug, ct);

        if (article is null)
            return Result<ArticleCommentsResult>.Failure(ArticleErrors.NotFound);

        var comments = await _commentRepository.GetByArticleIdAsync(article.Id, ct);

        return Result<ArticleCommentsResult>.Success(new ArticleCommentsResult(comments));
    }
}
