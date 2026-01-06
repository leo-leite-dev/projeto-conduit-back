using Conduit.Application.Abstractions.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Comments.Delete;

public sealed record DeleteCommentFromArticleCommand(string Slug, Guid CommentId)
    : IRequest<Result>;
