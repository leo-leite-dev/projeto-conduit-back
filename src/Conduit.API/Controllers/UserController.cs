using Conduit.Api.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers;

[ApiController]
[Authorize]
public sealed class UserController : ControllerBase
{
    private readonly IMediator _mediator;

    public UserController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("user")]
    public async Task<IActionResult> GetCurrentUser(CancellationToken ct)
    {
        var query = new GetCurrentUserQuery();
        var result = await _mediator.Send(query, ct);

        return result.ToActionResult(this);
    }
}
