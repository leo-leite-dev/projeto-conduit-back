using Conduit.Application.Abstractions.Auth;
using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using Conduit.Application.Abstractions.Time;
using Conduit.Application.Abstractions.UnitOfWork;
using Conduit.Application.Errors;
using Conduit.Application.Features.Articles.Commands.Create;
using Conduit.Application.Features.Articles.Results;
using Conduit.Domain.Entities;
using Conduit.Domain.Errors;
using MediatR;

public sealed class CreateArticleCommandHandler
    : IRequestHandler<CreateArticleCommand, Result<CreateArticleResult>>
{
    private readonly IArticleRepository _articleRepository;
    private readonly IProfileRepository _profileRepository;
    private readonly ICurrentUser _currentUser;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IDateTimeProvider _dateTime;

    public CreateArticleCommandHandler(
        IArticleRepository articleRepository,
        IProfileRepository profileRepository,
        ICurrentUser currentUser,
        IUnitOfWork unitOfWork,
        IDateTimeProvider dateTime
    )
    {
        _articleRepository = articleRepository;
        _profileRepository = profileRepository;
        _currentUser = currentUser;
        _unitOfWork = unitOfWork;
        _dateTime = dateTime;
    }

    public async Task<Result<CreateArticleResult>> Handle(
        CreateArticleCommand request,
        CancellationToken ct
    )
    {
        if (!_currentUser.IsAuthenticated)
            return Result<CreateArticleResult>.Failure(AuthErrors.Unauthorized);

        var author = await _profileRepository.GetByUsernameAsync(_currentUser.Username!, ct);

        if (author is null)
            return Result<CreateArticleResult>.Failure(ProfileErrors.NotFound);

        var article = Article.Create(
            request.Title,
            request.Description,
            request.Body,
            request.TagList,
            author,
            _dateTime.UtcNow
        );

        await _articleRepository.AddAsync(article, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return Result<CreateArticleResult>.Success(new CreateArticleResult(article));
    }
}
