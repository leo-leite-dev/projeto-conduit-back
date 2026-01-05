using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Errors;
using Conduit.Application.Features.Articles.Create;
using Conduit.Application.Tags;
using Conduit.Domain.Entities;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.CreateArticle;

public sealed class CreateArticleCommandHandler
    : IRequestHandler<CreateArticleCommand, Result<CreateArticleResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUser _currentUser;

    public CreateArticleCommandHandler(
        IArticleRepository articleRepository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser
    )
    {
        _articleRepository = articleRepository;
        _profileRepository = profileRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<CreateArticleResult>> Handle(
        CreateArticleCommand command,
        CancellationToken ct
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Result<CreateArticleResult>.Failure(AuthErrors.Unauthorized);

        if (command.TagList.Any(tag => !AvailableTags.All.Contains(tag)))
            return Result<CreateArticleResult>.Failure(ArticleErrors.InvalidTags);

        var author = await _profileRepository.GetByUsernameAsync(_currentUser.Username, ct);

        var article = Article.Create(
            command.Title,
            command.Description,
            command.Body,
            command.TagList,
            author!,
            DateTime.UtcNow
        );

        if (await _articleRepository.SlugExistsAsync(article.Slug, ct))
            return Result<CreateArticleResult>.Failure(ArticleErrors.SlugAlreadyExists);

        await _articleRepository.AddAsync(article, ct);

        return Result<CreateArticleResult>.Success(new CreateArticleResult(article));
    }
}
