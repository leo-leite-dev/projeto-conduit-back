using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Conduit.Infrastructure.Persistence.Context;

public sealed class ConduitDbContextFactory : IDesignTimeDbContextFactory<ConduitDbContext>
{
    public ConduitDbContext CreateDbContext(string[] args)
    {
        var environment =
            Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";

        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false)
            .AddJsonFile($"appsettings.{environment}.json", optional: true)
            .Build();

        var optionsBuilder = new DbContextOptionsBuilder<ConduitDbContext>();

        optionsBuilder.UseNpgsql(configuration.GetConnectionString("DefaultConnection"));

        return new ConduitDbContext(optionsBuilder.Options);
    }
}
