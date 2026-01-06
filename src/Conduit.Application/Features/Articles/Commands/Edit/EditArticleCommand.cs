using Conduit.Application.Abstractions.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Edit;

public sealed record EditArticleCommand(string Slug, string Title, string Description, string Body)
    : IRequest<Result>;
