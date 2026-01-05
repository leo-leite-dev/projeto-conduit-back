using Conduit.Application.Abstractions.Results;
using Conduit.Application.Tags;
using Conduit.Application.Tags.Queries;
using MediatR;

public sealed class GetTagsQueryHandler : IRequestHandler<GetTagsQuery, Result<GetTagsResult>>
{
    public Task<Result<GetTagsResult>> Handle(GetTagsQuery request, CancellationToken ct)
    {
        var result = new GetTagsResult(AvailableTags.All);

        return Task.FromResult(Result<GetTagsResult>.Success(result));
    }
}
