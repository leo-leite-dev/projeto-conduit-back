namespace Conduit.Api.Contracts.Articles.Comments;

public sealed record EditCommentRequest(EditCommentBody Comment);

public sealed record EditCommentBody(string Body);
