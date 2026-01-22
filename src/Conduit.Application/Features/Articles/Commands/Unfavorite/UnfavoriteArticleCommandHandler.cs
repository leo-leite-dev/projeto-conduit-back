using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Abstractions.UnitOfWork;
using Conduit.Application.Errors;
using Conduit.Application.Features.Articles.Results;
using Conduit.Domain.Errors;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Unfavorite;

public sealed class UnfavoriteArticleCommandHandler
    : IRequestHandler<UnfavoriteArticleCommand, Result<ArticleResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleFavoriteRepository _favoriteRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public UnfavoriteArticleCommandHandler(
        IArticleRepository articleRepository,
        IArticleFavoriteRepository favoriteRepository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork
    )
    {
        _articleRepository = articleRepository;
        _favoriteRepository = favoriteRepository;
        _profileRepository = profileRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<ArticleResult>> Handle(
        UnfavoriteArticleCommand command,
        CancellationToken ct
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Result<ArticleResult>.Failure(AuthErrors.Unauthorized);

        var article = await _articleRepository.GetBySlugAsync(command.Slug, ct);
        if (article is null)
            return Result<ArticleResult>.Failure(ArticleErrors.NotFound);

        var profile = await _profileRepository.GetByUsernameAsync(_currentUser.Username, ct);
        if (profile is null)
            return Result<ArticleResult>.Failure(AuthErrors.Unauthorized);

        var exists = await _favoriteRepository.ExistsAsync(article.Id, profile.Id, ct);

        if (exists)
        {
            await _favoriteRepository.RemoveAsync(article.Id, profile.Id, ct);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        var favorited = false;

        var favoritesCount = await _favoriteRepository.CountByArticleAsync(article.Id, ct);

        var isAuthorFollowed = false;

        var result = ArticleResultFactory.Create(
            article,
            favorited,
            favoritesCount,
            isAuthorFollowed
        );

        return Result<ArticleResult>.Success(result);
    }
}
