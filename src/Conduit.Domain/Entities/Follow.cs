namespace Conduit.Domain.Entities;

public sealed class Follow
{
    public Guid FollowerId { get; private set; }
    public Profile Follower { get; private set; } = null!;

    public Guid FollowedId { get; private set; }
    public Profile Followed { get; private set; } = null!;

    private Follow() { }

    private Follow(Profile follower, Profile followed)
    {
        Follower = follower;
        Followed = followed;
        FollowerId = follower.Id;
        FollowedId = followed.Id;
    }

    public static Follow Create(Profile follower, Profile followed)
    {
        return new Follow(follower, followed);
    }
}
