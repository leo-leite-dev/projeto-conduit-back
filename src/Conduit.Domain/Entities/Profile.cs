namespace Conduit.Domain.Entities;

public sealed class Profile
{
    public Guid Id { get; private set; }
    public string Username { get; private set; } = default!;
    public string Bio { get; private set; } = string.Empty;
    public string Image { get; private set; } = string.Empty;
    public bool Following { get; private set; }

    private Profile() { }

    private Profile(string username)
    {
        Id = Guid.NewGuid();
        Username = username;
        Bio = string.Empty;
        Image = string.Empty;
        Following = false;
    }

    public static Profile Create(string username)
    {
        return new Profile(username);
    }

    public void UpdateBio(string bio)
    {
        Bio = bio;
    }

    public void UpdateImage(string image)
    {
        Image = image;
    }

    public void Follow()
    {
        Following = true;
    }

    public void Unfollow()
    {
        Following = false;
    }
}
