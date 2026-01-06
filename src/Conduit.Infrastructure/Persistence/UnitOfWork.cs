using Conduit.Application.Abstractions.UnitOfWork;
using Conduit.Infrastructure.Persistence.Context;

namespace Conduit.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly ConduitDbContext _dbContext;

    public UnitOfWork(ConduitDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _dbContext.SaveChangesAsync(ct);
    }
}
