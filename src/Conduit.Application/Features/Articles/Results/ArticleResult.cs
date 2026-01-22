using Conduit.Application.Features.Profiles.Results;

namespace Conduit.Application.Features.Articles.Results;

public sealed record ArticleResult(
    string Slug,
    string Title,
    string Description,
    string Body,
    IReadOnlyList<string> TagList,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    bool Favorited,
    int FavoritesCount,
    ProfileResult Author
);
