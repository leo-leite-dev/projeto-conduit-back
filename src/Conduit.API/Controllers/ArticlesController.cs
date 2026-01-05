using Conduit.Api.Extensions;
using Conduit.Application.Features.Articles;
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
}
