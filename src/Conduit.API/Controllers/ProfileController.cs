using Conduit.Application.Features.Profiles.Commands.Follows;
using Conduit.Application.Features.Profiles.Commands.Unfollows;
using Conduit.Application.Features.Profiles.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.API.Controllers;

[ApiController]
[Route("profiles")]
public sealed class ProfileController : ControllerBase
{
    private readonly ISender _sender;

    public ProfileController(ISender sender)
    {
        _sender = sender;
    }

    [HttpGet("{username}")]
    public async Task<IActionResult> Get(string username) =>
        Ok(await _sender.Send(new GetProfileQuery(username)));

    [HttpPost("{username}/follow")]
    public async Task<IActionResult> Follow(string username) =>
        Ok(await _sender.Send(new FollowProfileCommand(username)));

    [HttpDelete("{username}/follow")]
    public async Task<IActionResult> Unfollow(string username) =>
        Ok(await _sender.Send(new UnfollowProfileCommand(username)));
}
