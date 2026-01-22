namespace Conduit.Domain.Entities;

public sealed class Profile
{
    private readonly List<Follow> _followers = new();

    public Guid Id { get; private set; }
    public string Username { get; private set; } = default!;
    public string Bio { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;

    // Navegação somente leitura
    public IReadOnlyCollection<Follow> Followers => _followers;

    private Profile() { }

    private Profile(string username)
    {
        Id = Guid.NewGuid();
        Username = username;
    }

    public static Profile Create(string username) => new(username);

    public void UpdateBio(string bio) => Bio = bio;

    public void UpdateImage(string image) => Image = image;

    public void AddFollower(Profile follower)
    {
        if (_followers.Any(f => f.FollowerId == follower.Id))
            return;

        _followers.Add(Follow.Create(follower, this));
    }

    public void RemoveFollower(Guid followerId)
    {
        var follow = _followers.FirstOrDefault(f => f.FollowerId == followerId);
        if (follow is not null)
            _followers.Remove(follow);
    }

    public bool IsFollowedBy(Guid followerId) => _followers.Any(f => f.FollowerId == followerId);
}
