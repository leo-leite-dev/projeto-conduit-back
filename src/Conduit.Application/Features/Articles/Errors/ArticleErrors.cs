using Conduit.Application.Abstractions.Results;

namespace Conduit.Application.Features.Articles;

public static class ArticleErrors
{
    public static readonly Error InvalidTags = new(
        "Article.InvalidTags",
        "One or more tags are invalid."
    );

    public static readonly Error SlugAlreadyExists = new(
        "Article.SlugAlreadyExists",
        "Article slug already exists."
    );

    public static readonly Error AuthorNotFound = new(
        "Article.AuthorNotFound",
        "Author profile was not found."
    );
}
