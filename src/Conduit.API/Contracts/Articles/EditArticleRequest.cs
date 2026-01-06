namespace Conduit.Api.Contracts.Articles;

public sealed record EditArticleRequest(EditArticleBody Article);

public sealed record EditArticleBody(string Title, string Description, string Body);
