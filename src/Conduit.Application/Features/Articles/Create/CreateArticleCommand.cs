using Conduit.Application.Abstractions.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Create;

public sealed record CreateArticleCommand(
    string Title,
    string Description,
    string Body,
    IReadOnlyList<string> TagList
) : IRequest<Result<CreateArticleResult>>;
