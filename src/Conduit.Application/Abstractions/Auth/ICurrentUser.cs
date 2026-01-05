using Conduit.Domain.Entities;

namespace Conduit.Application.Abstractions.Auth;

public interface ICurrentUser
{
    string Username { get; }
    bool IsAuthenticated { get; }
}
