using Conduit.Application.Features.Profiles.Results;
using Conduit.Domain.Entities;

namespace Conduit.Application.Features.Articles.Results;

public static class ArticleResultFactory
{
    public static ArticleResult Create(
        Article article,
        bool favorited,
        int favoritesCount,
        bool isAuthorFollowed
    )
    {
        return new ArticleResult(
            article.Slug,
            article.Title,
            article.Description,
            article.Body,
            article.TagList,
            article.CreatedAt,
            article.UpdatedAt,
            favorited,
            favoritesCount,
            ProfileResultFactory.Create(article.Author, isAuthorFollowed)
        );
    }
}
