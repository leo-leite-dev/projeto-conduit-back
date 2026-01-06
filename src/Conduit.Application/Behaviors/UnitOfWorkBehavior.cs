using Conduit.Application.Abstractions.UnitOfWork;
using MediatR;

namespace Conduit.Application.Behaviors;

public sealed class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
    private readonly IUnitOfWork _unitOfWork;

    public UnitOfWorkBehavior(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken ct
    )
    {
        var response = await next();

        await _unitOfWork.SaveChangesAsync(ct);

        return response;
    }
}
