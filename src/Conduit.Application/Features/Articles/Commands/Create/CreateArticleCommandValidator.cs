using Conduit.Application.Tags;
using FluentValidation;

namespace Conduit.Application.Features.Articles.Commands.Create;

public sealed class CreateArticleCommandValidator : AbstractValidator<CreateArticleCommand>
{
    public CreateArticleCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(150);

        RuleFor(x => x.Description).NotEmpty().MaximumLength(300);

        RuleFor(x => x.Body).NotEmpty();

        RuleFor(x => x.TagList).NotNull();

        RuleForEach(x => x.TagList)
            .Must(tag => AvailableTags.All.Contains(tag))
            .WithErrorCode(ArticleErrors.InvalidTags.Code)
            .WithMessage("Invalid tag.");
    }
}
