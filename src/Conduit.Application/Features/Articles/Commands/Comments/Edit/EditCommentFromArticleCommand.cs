using Conduit.Application.Abstractions.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Comments.Edit;

public sealed record EditCommentFromArticleCommand(string Slug, Guid CommentId, string Body)
    : IRequest<Result>;
