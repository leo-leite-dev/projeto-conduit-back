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
    }

    public Task UpdateAsync(Article article, CancellationToken ct)
    {
        _db.Articles.Update(article);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Article article, CancellationToken ct)
    {
        _db.Articles.Remove(article);
        return Task.CompletedTask;
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

    public async Task<Article?> GetBySlugAsync(string slug, CancellationToken ct)
    {
        return await _db
            .Articles.AsNoTracking()
            .Include(a => a.Author)
            .FirstOrDefaultAsync(a => a.Slug == slug, ct);
    }

    public async Task<IReadOnlyList<Article>> GetFeedAsync(
        string username,
        int limit,
        int offset,
        CancellationToken ct
    )
    {
        return await _db
            .Articles.AsNoTracking()
            .Where(article =>
                _db.Follows.Any(f =>
                    f.Follower.Username == username && f.Followed.Id == article.Author.Id
                )
            )
            .OrderByDescending(a => a.CreatedAt)
            .Skip(offset)
            .Take(limit)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct)
    {
        return await _db.Articles.CountAsync(ct);
    }

    public async Task<int> CountFeedAsync(string username, CancellationToken ct)
    {
        return await _db
            .Articles.AsNoTracking()
            .CountAsync(
                article =>
                    _db.Follows.Any(f =>
                        f.Follower.Username == username && f.Followed.Id == article.Author.Id
                    ),
                ct
            );
    }

    public async Task<bool> SlugExistsAsync(string slug, CancellationToken ct)
    {
        return await _db.Articles.AnyAsync(a => a.Slug == slug, ct);
    }
}
