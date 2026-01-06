using FluentValidation;

namespace Conduit.Application.Features.Articles.Commands.Comments.Delete;

public sealed class DeleteCommentFromArticleCommandValidator
    : AbstractValidator<DeleteCommentFromArticleCommand>
{
    public DeleteCommentFromArticleCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();

        RuleFor(x => x.CommentId).NotEmpty();
    }
}
