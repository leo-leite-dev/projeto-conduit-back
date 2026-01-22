namespace Conduit.Application.Abstractions.Auth;

public interface ICurrentUser
{
    bool IsAuthenticated { get; }
    string Username { get; }
}
