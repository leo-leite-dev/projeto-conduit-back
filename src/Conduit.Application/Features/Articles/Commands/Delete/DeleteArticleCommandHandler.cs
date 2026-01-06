using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Errors;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Delete;

public sealed class DeleteArticleCommandHandler : IRequestHandler<DeleteArticleCommand, Result>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ICurrentUser _currentUser;

    public DeleteArticleCommandHandler(
        IArticleRepository articleRepository,
        ICurrentUser currentUser
    )
    {
        _articleRepository = articleRepository;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(DeleteArticleCommand command, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated)
            return Result.Failure(AuthErrors.Unauthorized);

        var article = await _articleRepository.GetBySlugAsync(command.Slug, ct);
        if (article is null)
            return Result.Failure(ArticleErrors.NotFound);

        if (article.Author.Username != _currentUser.Username)
            return Result.Failure(ArticleErrors.ForbiddenArticleDeletion);

        await _articleRepository.DeleteAsync(article, ct);

        return Result.Success();
    }
}
