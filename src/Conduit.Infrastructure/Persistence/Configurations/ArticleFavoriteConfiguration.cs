using Conduit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Infrastructure.Persistence.Configurations;

public sealed class ArticleFavoriteConfiguration : IEntityTypeConfiguration<ArticleFavorite>
{
    public void Configure(EntityTypeBuilder<ArticleFavorite> builder)
    {
        builder.ToTable("article_favorites");

        builder.HasKey(af => new { af.ArticleId, af.ProfileId });

        builder.Property(af => af.ArticleId).IsRequired();

        builder.Property(af => af.ProfileId).IsRequired();

        builder
            .HasOne(af => af.Article)
            .WithMany()
            .HasForeignKey(af => af.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(af => af.Profile)
            .WithMany()
            .HasForeignKey(af => af.ProfileId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
