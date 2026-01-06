using Conduit.Domain.Entities;

namespace Conduit.Application.Abstractions.Repositories;

public interface IProfileRepository
{
    Task AddAsync(Profile profile, CancellationToken ct = default);
    Task<Profile?> GetByUsernameAsync(string username, CancellationToken ct = default);
}
