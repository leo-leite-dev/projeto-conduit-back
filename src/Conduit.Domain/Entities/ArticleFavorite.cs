namespace Conduit.Domain.Entities;

public sealed class ArticleFavorite
{
    public Guid ArticleId { get; private set; }
    public Article Article { get; private set; } = null!;

    public Guid ProfileId { get; private set; }
    public Profile Profile { get; private set; } = null!;

    private ArticleFavorite() { }

    private ArticleFavorite(Profile profile, Article article)
    {
        Profile = profile;
        Article = article;
        ProfileId = profile.Id;
        ArticleId = article.Id;
    }

    public static ArticleFavorite Create(Profile profile, Article article)
    {
        return new ArticleFavorite(profile, article);
    }
}
