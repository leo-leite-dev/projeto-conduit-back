using FluentValidation;

namespace Conduit.Application.Features.Articles.Commands.Comments.Add;

public sealed class AddCommentToArticleCommandValidator
    : AbstractValidator<AddCommentToArticleCommand>
{
    public AddCommentToArticleCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();

        RuleFor(x => x.Body)
            .NotEmpty()
            .WithErrorCode(ArticleErrors.InvalidComment.Code)
            .WithMessage("Comment body cannot be empty.");
    }
}
