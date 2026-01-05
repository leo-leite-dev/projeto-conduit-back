using Conduit.Domain.Entities;

namespace Conduit.Application.Abstractions.Repositories;

public interface IArticleRepository
{
    Task<IReadOnlyList<Article>> GetPagedAsync(int limit, int offset, CancellationToken ct);
    Task<int> CountAsync(CancellationToken ct);
}
