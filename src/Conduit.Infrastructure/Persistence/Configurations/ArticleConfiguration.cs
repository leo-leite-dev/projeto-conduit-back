using Conduit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ArticleConfiguration : IEntityTypeConfiguration<Article>
{
    public void Configure(EntityTypeBuilder<Article> builder)
    {
        builder.ToTable("Articles");

        builder.HasKey(a => a.Id);

        builder.Property(a => a.Id).ValueGeneratedNever();

        builder.Property(a => a.Slug).IsRequired().HasMaxLength(200);

        builder.HasIndex(a => a.Slug).IsUnique();

        builder.Property(a => a.Title).IsRequired().HasMaxLength(300);

        builder.Property(a => a.Description).IsRequired().HasMaxLength(500);

        builder.Property(a => a.Body).IsRequired();

        builder
            .Property(a => a.TagList)
            .HasConversion(
                tags => string.Join(',', tags),
                value => value.Split(',', StringSplitOptions.RemoveEmptyEntries)
            )
            .HasColumnName("Tags");

        builder.Property(a => a.CreatedAt).IsRequired();

        builder.Property(a => a.UpdatedAt).IsRequired();

        builder.Property(a => a.Favorited).IsRequired();

        builder.Property(a => a.FavoritesCount).IsRequired();

        builder
            .HasOne(a => a.Author)
            .WithMany()
            .HasForeignKey("AuthorId")
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<Guid>("AuthorId").IsRequired();
    }
}
