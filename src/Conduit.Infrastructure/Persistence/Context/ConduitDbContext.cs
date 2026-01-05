using Conduit.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Persistence.Context;

public sealed class ConduitDbContext : DbContext
{
    public ConduitDbContext(DbContextOptions<ConduitDbContext> options)
        : base(options) { }

    public DbSet<Article> Articles => Set<Article>();
    public DbSet<Profile> Profiles => Set<Profile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ConduitDbContext).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
