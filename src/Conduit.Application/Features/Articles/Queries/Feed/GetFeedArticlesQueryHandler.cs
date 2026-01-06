using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Errors;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.Feed;

public sealed class GetFeedArticlesQueryHandler
    : IRequestHandler<GetFeedArticlesQuery, Result<ArticlesResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ICurrentUser _currentUser;

    public GetFeedArticlesQueryHandler(
        IArticleRepository articleRepository,
        ICurrentUser currentUser
    )
    {
        _articleRepository = articleRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<ArticlesResult>> Handle(
        GetFeedArticlesQuery query,
        CancellationToken ct
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Result<ArticlesResult>.Failure(AuthErrors.Unauthorized);

        var articles = await _articleRepository.GetFeedAsync(
            _currentUser.Username,
            query.Limit,
            query.Offset,
            ct
        );

        var count = await _articleRepository.CountFeedAsync(_currentUser.Username, ct);

        return Result<ArticlesResult>.Success(new ArticlesResult(articles, count));
    }
}
