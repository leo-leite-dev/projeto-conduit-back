using Conduit.Application.Abstractions.Repositories;
using Conduit.Domain.Entities;
using Conduit.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Persistence.Repositories;

public sealed class CommentRepository : ICommentRepository
{
    private readonly ConduitDbContext _db;

    public CommentRepository(ConduitDbContext db)
    {
        _db = db;
    }

    public async Task AddAsync(Comment comment, CancellationToken ct)
    {
        await _db.Comments.AddAsync(comment, ct);
    }

    public void Remove(Comment comment)
    {
        _db.Comments.Remove(comment);
    }

    public async Task<Comment?> GetByIdAsync(Guid commentId, CancellationToken ct)
    {
        return await _db
            .Comments.Include(c => c.Author)
            .FirstOrDefaultAsync(c => c.Id == commentId, ct);
    }

    public async Task<IReadOnlyList<Comment>> GetByArticleIdAsync(
        Guid articleId,
        CancellationToken ct
    )
    {
        return await _db
            .Comments.AsNoTracking()
            .Include(c => c.Author)
            .Where(c => c.ArticleId == articleId)
            .OrderBy(c => c.CreatedAt)
            .ToListAsync(ct);
    }
}
