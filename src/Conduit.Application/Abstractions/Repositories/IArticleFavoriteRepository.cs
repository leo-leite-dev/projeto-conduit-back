using Conduit.Domain.Entities;

namespace Conduit.Application.Abstractions.Repositories;

public interface IArticleFavoriteRepository
{
    Task<bool> ExistsAsync(Guid articleId, Guid profileId, CancellationToken ct);
    Task AddAsync(ArticleFavorite favorite, CancellationToken ct);
    Task RemoveAsync(Guid articleId, Guid profileId, CancellationToken ct);
    Task<int> CountByArticleAsync(Guid articleId, CancellationToken ct);
}
