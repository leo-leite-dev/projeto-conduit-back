using Conduit.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public sealed class ProfileConfiguration : IEntityTypeConfiguration<Profile>
{
    public void Configure(EntityTypeBuilder<Profile> builder)
    {
        builder.ToTable("profiles");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id).ValueGeneratedNever();

        builder.Property(p => p.Username).IsRequired().HasMaxLength(100);

        builder.HasIndex(p => p.Username).IsUnique();

        builder.Property(p => p.Bio).HasMaxLength(500);

        builder.Property(p => p.Image).HasMaxLength(300);

        builder.Property(p => p.Following).IsRequired();
    }
}
