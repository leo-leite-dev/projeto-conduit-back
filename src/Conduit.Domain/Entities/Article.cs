namespace Conduit.Domain.Entities;

public sealed class Article
{
    public Guid Id { get; init; }
    public string Slug { get; init; } = default!;
    public string Title { get; init; } = default!;
    public string Description { get; init; } = default!;
    public string Body { get; init; } = default!;
    public DateTime CreatedAt { get; init; }
    public DateTime UpdatedAt { get; init; }
    public string AuthorUsername { get; init; } = default!;
}
