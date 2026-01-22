using Conduit.Application.Abstractions.Repositories;
using Conduit.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Persistence.Repositories;

public sealed class FollowRepository : IFollowRepository
{
    private readonly ConduitDbContext _db;

    public FollowRepository(ConduitDbContext db)
    {
        _db = db;
    }

    public async Task<bool> ExistsAsync(
        Guid followerProfileId,
        Guid followedProfileId,
        CancellationToken ct = default
    )
    {
        return await _db.Follows.AnyAsync(
            f => f.FollowerId == followerProfileId && f.FollowedId == followedProfileId,
            ct
        );
    }
}
