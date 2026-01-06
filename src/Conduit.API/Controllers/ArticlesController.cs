using Conduit.Api.Contracts.Articles;
using Conduit.Api.Extensions;
using Conduit.Api.Mappers;
using Conduit.Application.Features.Articles.Queries.Feed;
using Conduit.Application.Features.Articles.Queries.Global;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("articles")]
public sealed class ArticlesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ArticlesController(IMediator mediator)
    {
        _mediator = mediator;
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
    [Authorize]
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

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(
        [FromBody] CreateArticleRequest request,
        CancellationToken ct = default
    )
    {
        var command = ArticleMapper.ToCommand(request);

        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(this);
    }
}
