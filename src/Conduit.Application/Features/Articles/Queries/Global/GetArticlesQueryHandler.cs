using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Queries.Global;
using Conduit.Application.Features.Articles.Results;
using Conduit.Domain.Entities;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.List;

public sealed class GetArticlesQueryHandler
    : IRequestHandler<GetArticlesQuery, Result<ArticlesResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleFavoriteRepository _favoriteRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUser _currentUser;

    public GetArticlesQueryHandler(
        IArticleRepository articleRepository,
        IArticleFavoriteRepository favoriteRepository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser
    )
    {
        _articleRepository = articleRepository;
        _favoriteRepository = favoriteRepository;
        _profileRepository = profileRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<ArticlesResult>> Handle(GetArticlesQuery query, CancellationToken ct)
    {
        var articles = await _articleRepository.GetPagedAsync(query.Limit, query.Offset, ct);

        var total = await _articleRepository.CountAsync(ct);

        Profile? currentProfile = null;

        if (_currentUser.IsAuthenticated)
            currentProfile = await _profileRepository.GetByUsernameAsync(_currentUser.Username, ct);

        var results = new List<ArticleResult>(articles.Count);

        foreach (var article in articles)
        {
            bool favorited = false;

            if (currentProfile is not null)
            {
                favorited = await _favoriteRepository.ExistsAsync(
                    article.Id,
                    currentProfile.Id,
                    ct
                );
            }

            results.Add(
                ArticleResultFactory.Create(
                    article,
                    favorited,
                    favoritesCount: 0,
                    isAuthorFollowed: false
                )
            );
        }

        return Result<ArticlesResult>.Success(new ArticlesResult(results, total));
    }
}
