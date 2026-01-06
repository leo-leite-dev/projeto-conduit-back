using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Abstractions.UnitOfWork;
using Conduit.Application.Errors;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Comments.Delete;

public sealed class DeleteCommentFromArticleCommandHandler
    : IRequestHandler<DeleteCommentFromArticleCommand, Result>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteCommentFromArticleCommandHandler(
        IArticleRepository articleRepository,
        ICommentRepository commentRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork
    )
    {
        _articleRepository = articleRepository;
        _commentRepository = commentRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteCommentFromArticleCommand command, CancellationToken ct)
    {
        if (!_currentUser.IsAuthenticated)
            return Result.Failure(AuthErrors.Unauthorized);

        var article = await _articleRepository.GetBySlugAsync(command.Slug, ct);
        if (article is null)
            return Result.Failure(ArticleErrors.NotFound);

        var comment = await _commentRepository.GetByIdAsync(command.CommentId, ct);
        if (comment is null || comment.ArticleId != article.Id)
            return Result.Failure(ArticleErrors.CommentNotFound);

        if (comment.Author.Username != _currentUser.Username)
            return Result.Failure(ArticleErrors.ForbiddenCommentDeletion);

        _commentRepository.Remove(comment);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}
