using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Abstractions.UnitOfWork;
using Conduit.Application.Errors;
using Conduit.Application.Features.Articles.Results;
using Conduit.Domain.Entities;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Comments.Add;

public sealed class AddCommentToArticleCommandHandler
    : IRequestHandler<AddCommentToArticleCommand, Result<CommentResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public AddCommentToArticleCommandHandler(
        IArticleRepository articleRepository,
        ICommentRepository commentRepository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork
    )
    {
        _articleRepository = articleRepository;
        _commentRepository = commentRepository;
        _profileRepository = profileRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<CommentResult>> Handle(
        AddCommentToArticleCommand command,
        CancellationToken ct
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Result<CommentResult>.Failure(AuthErrors.Unauthorized);

        var article = await _articleRepository.GetBySlugAsync(command.Slug, ct);
        if (article is null)
            return Result<CommentResult>.Failure(ArticleErrors.NotFound);

        var author = await _profileRepository.GetByUsernameAsync(_currentUser.Username, ct);

        var comment = Comment.Create(command.Body.Trim(), article.Id, author!, DateTime.UtcNow);

        await _commentRepository.AddAsync(comment, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<CommentResult>.Success(new CommentResult(comment));
    }
}
