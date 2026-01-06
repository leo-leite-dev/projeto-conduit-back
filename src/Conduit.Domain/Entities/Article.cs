using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace Conduit.Domain.Entities;

public sealed class Article
{
    public Guid Id { get; private init; }
    public string Slug { get; private init; } = default!;
    public string Title { get; private set; } = default!;
    public string Description { get; private set; } = default!;
    public string Body { get; private set; } = default!;
    public IReadOnlyList<string> TagList { get; private init; } = [];
    public DateTime CreatedAt { get; private init; }
    public DateTime UpdatedAt { get; private set; }
    public bool Favorited { get; private set; }
    public int FavoritesCount { get; private set; }
    public Profile Author { get; private init; } = default!;

    private Article() { }

    public static Article Create(
        string title,
        string description,
        string body,
        IReadOnlyList<string> tags,
        Profile author,
        DateTime now
    )
    {
        return new Article
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Body = body,
            Slug = GenerateSlug(title),
            TagList = tags,
            CreatedAt = now,
            UpdatedAt = now,
            Favorited = false,
            FavoritesCount = 0,
            Author = author,
        };
    }

    public void Update(string title, string description, string body, DateTime now)
    {
        Title = title;
        Description = description;
        Body = body;
        UpdatedAt = now;
    }

    private static string GenerateSlug(string title)
    {
        var normalized = title.ToLowerInvariant().Normalize(NormalizationForm.FormD);

        var sb = new StringBuilder();

        foreach (var c in normalized)
        {
            var category = Char.GetUnicodeCategory(c);

            if (category == UnicodeCategory.NonSpacingMark)
                continue;

            if (char.IsLetterOrDigit(c))
                sb.Append(c);
            else if (c == ' ' || c == '-')
                sb.Append('-');
        }

        var slug = Regex.Replace(sb.ToString(), "-{2,}", "-").Trim('-');

        return slug;
    }
}
