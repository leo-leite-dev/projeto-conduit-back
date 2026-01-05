using Conduit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Infrastructure.Persistence.Configurations;

public sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("articles");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Slug).IsRequired().HasMaxLength(200);

        builder.HasIndex(a => a.Slug).IsUnique();

        builder.Property(a => a.Title).IsRequired().HasMaxLength(250);

        builder.Property(a => a.Description).IsRequired().HasMaxLength(500);

        builder.Property(a => a.Body).IsRequired();

        builder.Property(a => a.AuthorUsername).IsRequired().HasMaxLength(100);

        builder.Property(a => a.CreatedAt).IsRequired();

        builder.Property(a => a.UpdatedAt).IsRequired();
    }
}
