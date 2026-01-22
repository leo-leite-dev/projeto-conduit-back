using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Abstractions.UnitOfWork;
using Conduit.Application.Errors;
using Conduit.Application.Features.Profiles.Results;
using Conduit.Domain.Entities;
using Conduit.Domain.Errors;
using MediatR;

public sealed class CreateProfileCommandHandler
    : IRequestHandler<CreateProfileCommand, Result<GetProfileResponse>>
{
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;

    public CreateProfileCommandHandler(
        IProfileRepository profileRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork
    )
    {
        _profileRepository = profileRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<GetProfileResponse>> Handle(
        CreateProfileCommand command,
        CancellationToken ct
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Result<GetProfileResponse>.Failure(AuthErrors.Unauthorized);

        var existing = await _profileRepository.GetByUsernameAsync(command.Username, ct);

        if (existing is not null)
            return Result<GetProfileResponse>.Failure(ProfileErrors.AlreadyExists);

        var profile = Profile.Create(command.Username);

        await _profileRepository.AddAsync(profile, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<GetProfileResponse>.Success(
            new GetProfileResponse
            {
                Username = profile.Username,
                Bio = profile.Bio,
                Image = profile.Image,
                Following = false,
            }
        );
    }
}
