using FluentValidation;

namespace Conduit.Application.Features.Articles.Commands.Favorite;

public sealed class FavoriteArticleCommandValidator : AbstractValidator<FavoriteArticleCommand>
{
    public FavoriteArticleCommandValidator()
    {
        RuleFor(x => x.Slug).NotEmpty().WithMessage("Article slug is required.");
    }
}
