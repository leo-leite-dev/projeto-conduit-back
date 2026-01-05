using Conduit.Application.Abstractions.Repositories;
using Conduit.Domain.Entities;
using Conduit.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Persistence.Repositories;

public sealed class ArticleRepository : IArticleRepository
{
    private readonly ConduitDbContext _db;

    public ArticleRepository(ConduitDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Article article, CancellationToken ct)
    {
        await _db.Articles.AddAsync(article, ct);
        await _db.SaveChangesAsync(ct);
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

    public async Task<bool> SlugExistsAsync(string slug, CancellationToken ct)
    {
        return await _db.Articles.AnyAsync(a => a.Slug == slug, ct);
    }
}
