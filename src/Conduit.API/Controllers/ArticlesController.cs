using Conduit.Api.Contracts.Articles;
using Conduit.Api.Mappers;
using Conduit.Application.Features.Articles.Commands.Delete;
using Conduit.Application.Features.Articles.Commands.Favorite;
using Conduit.Application.Features.Articles.Commands.Unfavorite;
using Conduit.Application.Features.Articles.Queries.Details;
using Conduit.Application.Features.Articles.Queries.Global;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers;

[ApiController]
[Route("articles")]
public sealed class ArticlesController : ControllerBase
{
    private readonly ISender _sender;

    public ArticlesController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet]
    public async Task<IActionResult> Get([FromQuery] GetArticlesQuery query) =>
        Ok(await _sender.Send(query));

    [HttpGet("{slug}")]
    public async Task<IActionResult> GetBySlug(string slug) =>
        Ok(await _sender.Send(new GetArticleBySlugQuery(slug)));

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticleRequest request)
    {
        var command = ArticleMapper.ToCommand(request);
        return Ok(await _sender.Send(command));
    }

    [HttpPut("{slug}")]
    public async Task<IActionResult> Edit(string slug, [FromBody] EditArticleRequest request)
    {
        var command = ArticleMapper.ToCommand(slug, request);
        return Ok(await _sender.Send(command));
    }

    [HttpDelete("{slug}")]
    public async Task<IActionResult> Delete(string slug)
    {
        await _sender.Send(new DeleteArticleCommand(slug));
        return NoContent();
    }

    [HttpPost("{slug}/favorite")]
    public async Task<IActionResult> Favorite(string slug) =>
        Ok(await _sender.Send(new FavoriteArticleCommand(slug)));

    [HttpDelete("{slug}/favorite")]
    public async Task<IActionResult> Unfavorite(string slug) =>
        Ok(await _sender.Send(new UnfavoriteArticleCommand(slug)));
}
