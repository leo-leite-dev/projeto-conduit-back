using Conduit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Infrastructure.Persistence.Configurations;

public sealed class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments");

        builder.HasKey(c => c.Id);

        builder.Property(c => c.Id).ValueGeneratedNever();

        builder.Property(c => c.Body).IsRequired().HasMaxLength(1000);

        builder.Property(c => c.CreatedAt).IsRequired();

        builder.Property(c => c.ArticleId).IsRequired();

        builder
            .HasOne<Article>()
            .WithMany()
            .HasForeignKey(c => c.ArticleId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(c => c.Author)
            .WithMany()
            .HasForeignKey("AuthorId")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}
