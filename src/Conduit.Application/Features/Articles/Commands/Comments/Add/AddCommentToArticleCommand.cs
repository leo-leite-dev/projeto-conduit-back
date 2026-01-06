using Conduit.Application.Abstractions.Results;
using Conduit.Application.Features.Articles.Results;
using MediatR;

namespace Conduit.Application.Features.Articles.Commands.Comments.Add;

public sealed record AddCommentToArticleCommand(string Slug, string Body)
    : IRequest<Result<CommentResult>>;
