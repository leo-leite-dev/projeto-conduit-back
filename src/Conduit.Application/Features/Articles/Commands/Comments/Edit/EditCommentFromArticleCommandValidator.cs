using Conduit.Application.Errors;
using FluentValidation;

namespace Conduit.Application.Features.Articles.Commands.Comments.Edit;

public sealed class EditCommentFromArticleCommandValidator
    : AbstractValidator<EditCommentFromArticleCommand>
{
    public EditCommentFromArticleCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();

        RuleFor(x => x.CommentId).NotEmpty();

        RuleFor(x => x.Body)
            .NotEmpty()
            .WithErrorCode(ArticleErrors.InvalidComment.Code)
            .WithMessage("Comment body cannot be empty.");
    }
}
