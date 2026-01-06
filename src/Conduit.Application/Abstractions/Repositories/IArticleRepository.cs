using Conduit.Domain.Entities;

namespace Conduit.Application.Abstractions.Repositories;

public interface IArticleRepository
{
    Task AddAsync(Article article, CancellationToken ct);
    Task UpdateAsync(Article article, CancellationToken ct);
    Task DeleteAsync(Article article, CancellationToken ct);
    Task<IReadOnlyList<Article>> GetPagedAsync(int limit, int offset, CancellationToken ct);
    Task<Article?> GetBySlugAsync(string slug, CancellationToken ct);
    Task<IReadOnlyList<Article>> GetFeedAsync(
        string username,
        int limit,
        int offset,
        CancellationToken ct
    );
    Task<int> CountAsync(CancellationToken ct);
    Task<int> CountFeedAsync(string username, CancellationToken ct);
    Task<bool> SlugExistsAsync(string slug, CancellationToken ct);
}
