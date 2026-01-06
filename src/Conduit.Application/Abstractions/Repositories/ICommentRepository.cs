using Conduit.Domain.Entities;

namespace Conduit.Application.Abstractions.Repositories;

public interface ICommentRepository
{
    Task AddAsync(Comment comment, CancellationToken ct);
    void Remove(Comment comment);

    Task<Comment?> GetByIdAsync(Guid commentId, CancellationToken ct);
    Task<IReadOnlyList<Comment>> GetByArticleIdAsync(Guid articleId, CancellationToken ct);
}
