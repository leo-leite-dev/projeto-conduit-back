using Conduit.Application.Abstractions.Repositories;
using Conduit.Domain.Entities;
using Conduit.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Persistence.Articles;

public sealed class ArticleRepository : IArticleRepository
{
    private readonly ConduitDbContext _db;

    public ArticleRepository(ConduitDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<Article>> GetPagedAsync(
        int limit,
        int offset,
        CancellationToken ct
    )
    {
        return await _db
            .Articles.AsNoTracking()
            .OrderByDescending(a => a.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct)
    {
        return await _db.Articles.CountAsync(ct);
    }
}
