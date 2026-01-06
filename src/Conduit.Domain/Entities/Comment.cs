namespace Conduit.Domain.Entities;

public sealed class Comment
{
    public Guid Id { get; private init; }
    public string Body { get; private set; } = default!;
    public DateTime CreatedAt { get; private init; }

    public Guid ArticleId { get; private init; }
    public Profile Author { get; private init; } = default!;

    private Comment() { }

    public static Comment Create(string body, Guid articleId, Profile author, DateTime now)
    {
        return new Comment
        {
            Id = Guid.NewGuid(),
            Body = body,
            ArticleId = articleId,
            Author = author,
            CreatedAt = now,
        };
    }

    public void UpdateBody(string body)
    {
        Body = body;
    }
}
