using FluentValidation;

namespace Conduit.Application.Features.Articles.Commands.Edit;

public sealed class EditArticleCommandValidator : AbstractValidator<EditArticleCommand>
{
    public EditArticleCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Body).NotEmpty();
    }
}
