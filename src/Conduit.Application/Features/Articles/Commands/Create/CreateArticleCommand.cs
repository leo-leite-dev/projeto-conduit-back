using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Create;

public sealed record CreateArticleCommand(
    string Title,
    string Description,
    string Body,
    IReadOnlyList<string> TagList
) : IRequest<Result<CreateArticleResult>>;
