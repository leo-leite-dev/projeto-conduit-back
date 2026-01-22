using Conduit.Api.Contracts.Articles;
using Conduit.Application.Features.Articles.Commands.Create;
using Conduit.Application.Features.Articles.Commands.Edit;

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

    public static EditArticleCommand ToCommand(string slug, EditArticleRequest request)
    {
        return new EditArticleCommand(
            slug,
            request.Article.Title,
            request.Article.Description,
            request.Article.Body
        );
    }
}
