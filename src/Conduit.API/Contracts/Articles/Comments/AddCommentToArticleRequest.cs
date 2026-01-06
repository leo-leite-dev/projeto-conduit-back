namespace Conduit.Api.Contracts.Articles.Comments;

public sealed class AddCommentToArticleRequest
{
    public AddCommentDto Comment { get; init; } = default!;
}

public sealed class AddCommentDto
{
    public string Body { get; init; } = default!;
}
