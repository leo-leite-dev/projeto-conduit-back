using Conduit.Application.Abstractions.Repositories;

namespace Conduit.Application.Features.Articles;

using Conduit.Application.Abstractions.Results;
using MediatR;

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
