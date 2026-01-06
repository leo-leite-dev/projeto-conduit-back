using Conduit.Api.Contracts.Articles.Comments;
using Conduit.Application.Features.Articles.Commands.Comments.Add;

namespace Conduit.Api.Mappers;

public static class CommentMapper
{
    public static AddCommentToArticleCommand ToCommand(
        string slug,
        AddCommentToArticleRequest request
    )
    {
        return new AddCommentToArticleCommand(slug, request.Comment.Body);
    }
}
