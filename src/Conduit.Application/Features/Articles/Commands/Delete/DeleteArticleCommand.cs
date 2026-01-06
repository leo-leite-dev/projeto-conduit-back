using Conduit.Application.Abstractions.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Delete;

public sealed record DeleteArticleCommand(string Slug) : IRequest<Result>;
