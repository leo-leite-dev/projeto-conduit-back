namespace Conduit.Application.Abstractions.UnitOfWork;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken ct = default);
}
