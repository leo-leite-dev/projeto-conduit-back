using Conduit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Conduit.Infrastructure.Persistence.Configurations;

public sealed class FollowConfiguration : IEntityTypeConfiguration<Follow>
{
    public void Configure(EntityTypeBuilder<Follow> builder)
    {
        builder.ToTable(
            "follows",
            table =>
            {
                table.HasCheckConstraint(
                    "CK_Follows_Follower_Followed",
                    "\"FollowerId\" <> \"FollowedId\""
                );
            }
        );

        builder.HasKey(f => new { f.FollowerId, f.FollowedId });

        builder.Property(f => f.FollowerId).IsRequired();

        builder.Property(f => f.FollowedId).IsRequired();

        builder
            .HasOne(f => f.Follower)
            .WithMany()
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(f => f.Followed)
            .WithMany()
            .HasForeignKey(f => f.FollowedId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
