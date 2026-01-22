using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Profiles.Results;
using Conduit.Domain.Errors;
using MediatR;

namespace Conduit.Application.Features.Profiles.Queries;

public sealed class GetProfileQueryHandler
    : IRequestHandler<GetProfileQuery, Result<GetProfileResponse>>
{
    private readonly IProfileRepository _profileRepository;

    public GetProfileQueryHandler(IProfileRepository profileRepository)
    {
        _profileRepository = profileRepository;
    }

    public async Task<Result<GetProfileResponse>> Handle(
        GetProfileQuery query,
        CancellationToken ct
    )
    {
        var profile = await _profileRepository.GetByUsernameAsync(query.Username, ct);

        if (profile is null)
            return Result<GetProfileResponse>.Failure(ProfileErrors.NotFound);

        var response = new GetProfileResponse
        {
            Username = profile.Username,
            Bio = profile.Bio,
            Image = profile.Image,
            Following = false,
        };

        return Result<GetProfileResponse>.Success(response);
    }
}
