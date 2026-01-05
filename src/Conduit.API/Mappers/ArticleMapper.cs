using Conduit.Api.Contracts.Articles;
using Conduit.Application.Features.Articles.Create;

namespace Conduit.Api.Mappers;

public static class ArticleMapper
{
    public static CreateArticleCommand ToCommand(CreateArticleRequest request)
    {
        return new CreateArticleCommand(
            request.Article.Title,
            request.Article.Description,
            request.Article.Body,
            request.Article.TagList
        );
    }
}
