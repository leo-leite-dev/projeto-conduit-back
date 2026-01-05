namespace Conduit.Api.Contracts.Articles;

public sealed record CreateArticleRequest(CreateArticleDto Article);

public sealed record CreateArticleDto(
    string Title,
    string Description,
    string Body,
    IReadOnlyList<string> TagList
);
