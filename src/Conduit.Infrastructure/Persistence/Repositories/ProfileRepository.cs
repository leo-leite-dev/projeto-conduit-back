using Conduit.Application.Abstractions.Repositories;
using Conduit.Domain.Entities;
using Conduit.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Infrastructure.Persistence.Repositories;

public sealed class ProfileRepository : IProfileRepository
{
    private readonly ConduitDbContext _context;

    public ProfileRepository(ConduitDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Profile profile, CancellationToken ct = default)
    {
        await _context.Profiles.AddAsync(profile, ct);
    }

    public Task UpdateAsync(Profile profile, CancellationToken ct = default)
    {
        _context.Profiles.Update(profile);
        return Task.CompletedTask;
    }

    public async Task<Profile?> GetByUsernameAsync(string username, CancellationToken ct = default)
    {
        return await _context
            .Profiles.AsNoTracking()
            .FirstOrDefaultAsync(p => p.Username == username, ct);
    }

    public async Task<Profile?> GetTrackedByUsernameAsync(
        string username,
        CancellationToken ct = default
    )
    {
        return await _context.Profiles.FirstOrDefaultAsync(p => p.Username == username, ct);
    }
}
