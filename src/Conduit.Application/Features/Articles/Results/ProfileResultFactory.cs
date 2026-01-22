using Conduit.Domain.Entities;

namespace Conduit.Application.Features.Profiles.Results;

public static class ProfileResultFactory
{
    public static ProfileResult Create(Profile profile, bool following)
    {
        return new ProfileResult(profile.Username, profile.Bio, profile.Image, following);
    }
}
