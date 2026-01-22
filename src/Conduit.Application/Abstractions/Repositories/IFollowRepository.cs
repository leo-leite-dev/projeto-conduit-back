namespace Conduit.Application.Abstractions.Repositories;

public interface IFollowRepository
{
    Task<bool> ExistsAsync(
        Guid followerProfileId,
        Guid followedProfileId,
        CancellationToken ct = default
    );
}
