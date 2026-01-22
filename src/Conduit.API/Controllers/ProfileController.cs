using Conduit.Api.Extensions;
using Conduit.Application.Features.Profiles.Commands.Follows;
using Conduit.Application.Features.Profiles.Commands.Unfollows;
using Conduit.Application.Features.Profiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers;

[ApiController]
[Route("api/profiles")]
public sealed class ProfileController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProfileController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{username}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetProfile([FromRoute] string username, CancellationToken ct)
    {
        var query = new GetProfileQuery(username);
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(this);
    }

    [HttpPost("{username}/follow")]
    [Authorize]
    public async Task<IActionResult> Follow([FromRoute] string username, CancellationToken ct)
    {
        var command = new FollowProfileCommand(username);
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(this);
    }

    [HttpDelete("{username}/follow")]
    [Authorize]
    public async Task<IActionResult> Unfollow([FromRoute] string username, CancellationToken ct)
    {
        var command = new UnfollowProfileCommand(username);
        var result = await _mediator.Send(command, ct);

        return result.ToActionResult(this);
    }
}
