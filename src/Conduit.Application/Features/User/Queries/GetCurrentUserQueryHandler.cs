using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Errors;
using Conduit.Application.User.Results;
using MediatR;

public sealed class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, Result<GetCurrentUserResponse>>
{
    private readonly ICurrentUser _currentUser;
    private readonly IProfileRepository _profileRepository;

    public GetCurrentUserQueryHandler(
        ICurrentUser currentUser,
        IProfileRepository profileRepository
    )
    {
        _currentUser = currentUser;
        _profileRepository = profileRepository;
    }

    public async Task<Result<GetCurrentUserResponse>> Handle(
        GetCurrentUserQuery query,
        CancellationToken ct
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Result<GetCurrentUserResponse>.Failure(AuthErrors.Unauthorized);

        var profile = await _profileRepository.GetByUsernameAsync(_currentUser.Username, ct);

        var response = new GetCurrentUserResponse(
            _currentUser.Username,
            profile?.Bio,
            profile?.Image
        );

        return Result<GetCurrentUserResponse>.Success(response);
    }
}
