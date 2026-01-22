using Conduit.Application.Abstractions.Repositories;
using Conduit.Domain.Entities;
using Conduit.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Persistence.Repositories;

public sealed class ArticleFavoriteRepository : IArticleFavoriteRepository
{
    private readonly ConduitDbContext _db;

    public ArticleFavoriteRepository(ConduitDbContext db)
    {
        _db = db;
    }

    public async Task<bool> ExistsAsync(Guid articleId, Guid profileId, CancellationToken ct)
    {
        return await _db.ArticleFavorites.AnyAsync(
            f => f.ArticleId == articleId && f.ProfileId == profileId,
            ct
        );
    }

    public async Task AddAsync(ArticleFavorite favorite, CancellationToken ct)
    {
        await _db.ArticleFavorites.AddAsync(favorite, ct);
    }

    public async Task RemoveAsync(Guid articleId, Guid profileId, CancellationToken ct)
    {
        var favorite = await _db.ArticleFavorites.FirstOrDefaultAsync(
            f => f.ArticleId == articleId && f.ProfileId == profileId,
            ct
        );

        if (favorite is not null)
            _db.ArticleFavorites.Remove(favorite);
    }

    public async Task<int> CountByArticleAsync(Guid articleId, CancellationToken ct)
    {
        return await _db.ArticleFavorites.CountAsync(f => f.ArticleId == articleId, ct);
    }
}
