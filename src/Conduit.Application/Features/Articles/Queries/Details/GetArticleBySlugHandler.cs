using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using Conduit.Domain.Errors;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries.Details;

public sealed class GetArticleBySlugQueryHandler
    : IRequestHandler<GetArticleBySlugQuery, Result<ArticleResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleFavoriteRepository _favoriteRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly IFollowRepository _followRepository;
    private readonly ICurrentUser _currentUser;

    public GetArticleBySlugQueryHandler(
        IArticleRepository articleRepository,
        IArticleFavoriteRepository favoriteRepository,
        IProfileRepository profileRepository,
        IFollowRepository followRepository,
        ICurrentUser currentUser
    )
    {
        _articleRepository = articleRepository;
        _favoriteRepository = favoriteRepository;
        _profileRepository = profileRepository;
        _followRepository = followRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<ArticleResult>> Handle(
        GetArticleBySlugQuery query,
        CancellationToken ct
    )
    {
        var article = await _articleRepository.GetBySlugAsync(query.Slug, ct);

        if (article is null)
            return Result<ArticleResult>.Failure(ArticleErrors.NotFound);

        var favoritesCount = await _favoriteRepository.CountByArticleAsync(article.Id, ct);

        bool favorited = false;
        bool isAuthorFollowed = false;

        if (_currentUser.IsAuthenticated)
        {
            var viewerProfile = await _profileRepository.GetByUsernameAsync(
                _currentUser.Username,
                ct
            );

            if (viewerProfile is not null)
            {
                favorited = await _favoriteRepository.ExistsAsync(article.Id, viewerProfile.Id, ct);

                isAuthorFollowed = await _followRepository.ExistsAsync(
                    viewerProfile.Id,
                    article.Author.Id,
                    ct
                );
            }
        }

        return Result<ArticleResult>.Success(
            ArticleResultFactory.Create(article, favorited, favoritesCount, isAuthorFollowed)
        );
    }
}
