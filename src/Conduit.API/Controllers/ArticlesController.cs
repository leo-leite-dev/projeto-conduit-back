using Conduit.Api.Contracts.Articles;
using Conduit.Api.Contracts.Articles.Comments;
using Conduit.Api.Extensions;
using Conduit.Api.Mappers;
using Conduit.Application.Features.Articles.Commands.Comments.Delete;
using Conduit.Application.Features.Articles.Commands.Comments.Edit;
using Conduit.Application.Features.Articles.Commands.Delete;
using Conduit.Application.Features.Articles.Commands.Edit;
using Conduit.Application.Features.Articles.Queries.Comments;
using Conduit.Application.Features.Articles.Queries.Details;
using Conduit.Application.Features.Articles.Queries.Feed;
using Conduit.Application.Features.Articles.Queries.Global;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("articles")]
[Authorize]
public sealed class ArticlesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticlesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateArticleRequest request,
        CancellationToken ct = default
    )
    {
        var command = ArticleMapper.ToCommand(request);
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(this);
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Edit(
        [FromRoute] string slug,
        [FromBody] EditArticleRequest request,
        CancellationToken ct = default
    )
    {
        var command = new EditArticleCommand(
            slug,
            request.Article.Title,
            request.Article.Description,
            request.Article.Body
        );

        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(this);
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete([FromRoute] string slug, CancellationToken ct = default)
    {
        var command = new DeleteArticleCommand(slug);
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(this);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Get(
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0,
        CancellationToken ct = default
    )
    {
        var query = new GetArticlesQuery(limit, offset);
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(this);
    }

    [HttpGet("feed")]
    public async Task<IActionResult> GetFeed(
        [FromQuery] int limit = 20,
        [FromQuery] int offset = 0,
        CancellationToken ct = default
    )
    {
        var query = new GetFeedArticlesQuery(limit, offset);
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(this);
    }

    [HttpGet("{slug}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBySlug(
        [FromRoute] string slug,
        CancellationToken ct = default
    )
    {
        var query = new GetArticleBySlugQuery(slug);
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(this);
    }

    [HttpPost("{slug}/comments")]
    public async Task<IActionResult> AddComment(
        string slug,
        AddCommentToArticleRequest request,
        CancellationToken ct
    )
    {
        var command = CommentMapper.ToCommand(slug, request);
        var result = await _mediator.Send(command, ct);

        return Ok(result);
    }

    [HttpPut("{slug}/comments/{commentId:guid}")]
    public async Task<IActionResult> EditComment(
        [FromRoute] string slug,
        [FromRoute] Guid commentId,
        [FromBody] EditCommentRequest request,
        CancellationToken ct = default
    )
    {
        var command = new EditCommentFromArticleCommand(slug, commentId, request.Comment.Body);
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(this);
    }

    [HttpDelete("{slug}/comments/{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(
        [FromRoute] string slug,
        [FromRoute] Guid commentId,
        CancellationToken ct = default
    )
    {
        var command = new DeleteCommentFromArticleCommand(slug, commentId);
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(this);
    }

    [HttpGet("{slug}/comments")]
    [AllowAnonymous]
    public async Task<IActionResult> GetComments(
        [FromRoute] string slug,
        CancellationToken ct = default
    )
    {
        var query = new GetArticleCommentsQuery(slug);
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(this);
    }
}
