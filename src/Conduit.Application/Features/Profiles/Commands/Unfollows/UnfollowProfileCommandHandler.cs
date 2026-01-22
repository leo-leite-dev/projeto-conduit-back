using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Errors;
using Conduit.Application.Features.Profiles.Results;
using Conduit.Domain.Errors;
using MediatR;

namespace Conduit.Application.Features.Profiles.Commands.Unfollows;

public sealed class UnfollowProfileCommandHandler
    : IRequestHandler<UnfollowProfileCommand, Result<GetProfileResponse>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUser _currentUser;

    public UnfollowProfileCommandHandler(
        IProfileRepository profileRepository,
        ICurrentUser currentUser
    )
    {
        _profileRepository = profileRepository;
        _currentUser = currentUser;
    }

    public async Task<Result<GetProfileResponse>> Handle(
        UnfollowProfileCommand command,
        CancellationToken ct
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Result<GetProfileResponse>.Failure(AuthErrors.Unauthorized);

        var followed = await _profileRepository.GetTrackedByUsernameAsync(command.Username, ct);

        if (followed is null)
            return Result<GetProfileResponse>.Failure(ProfileErrors.NotFound);

        var follower = await _profileRepository.GetByUsernameAsync(_currentUser.Username, ct);

        if (follower is null)
            return Result<GetProfileResponse>.Failure(ProfileErrors.NotFound);

        followed.RemoveFollower(follower.Id);

        await _profileRepository.UpdateAsync(followed, ct);

        var response = new GetProfileResponse
        {
            Username = followed.Username,
            Bio = followed.Bio,
            Image = followed.Image,
            Following = false,
        };

        return Result<GetProfileResponse>.Success(response);
    }
}
