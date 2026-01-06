using FluentValidation;

namespace Conduit.Application.Features.Articles.Commands.Delete;

public sealed class DeleteArticleCommandValidator : AbstractValidator<DeleteArticleCommand>
{
    public DeleteArticleCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
    }
}
