using FluentValidation;

namespace Conduit.Application.Features.Articles.Commands.Unfavorite;

public sealed class UnfavoriteArticleCommandValidator : AbstractValidator<UnfavoriteArticleCommand>
{
    public UnfavoriteArticleCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Article slug is required.");
    }
}
