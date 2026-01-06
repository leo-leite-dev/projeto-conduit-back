using Conduit.Application.Abstractions.Results;

namespace Conduit.Application.Features.Articles;

public static class ArticleErrors
{
    public static readonly Error ForbiddenArticleEdit = new(
        "Article.ForbiddenEdit",
        "You cannot edit this article."
    );

    public static readonly Error ForbiddenArticleDeletion = new(
        "Article.ForbiddenDeletion",
        "You cannot delete this article."
    );

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

    public static readonly Error NotFound = new("Article.NotFound", "Article was not found.");

    public static readonly Error InvalidComment = new(
        "Article.InvalidComment",
        "Comment body cannot be empty."
    );

    public static readonly Error CommentNotFound = new(
        "Article.CommentNotFound",
        "Comment not found."
    );

    public static readonly Error ForbiddenCommentEdit = new(
        "Article.ForbiddenCommentEdit",
        "You cannot edit this comment."
    );

    public static readonly Error ForbiddenCommentDeletion = new(
        "Article.ForbiddenCommentDeletion",
        "You cannot delete this comment."
    );
}
