using Conduit.Application.Abstractions.Repositories;
using Conduit.Application.Abstractions.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Queries;

public sealed class GetArticlesQueryHandler
    : IRequestHandler<GetArticlesQuery, Result<ArticlesResult>>
{
    private readonly IArticleRepository _repository;

    public GetArticlesQueryHandler(IArticleRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result<ArticlesResult>> Handle(GetArticlesQuery query, CancellationToken ct)
    {
        var articles = await _repository.GetPagedAsync(query.Limit, query.Offset, ct);

        var count = await _repository.CountAsync(ct);

        return Result<ArticlesResult>.Success(new ArticlesResult(articles, count));
    }
}
