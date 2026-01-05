using Conduit.Api.Authentication;
using Conduit.Api.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers;

[ApiController]
[Route("")]
public sealed class UsersController : ControllerBase
{
    private readonly AuthServiceClient _authClient;

    public UsersController(AuthServiceClient authClient)
    {
        _authClient = authClient;
    }

    [HttpPost("users")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(RegisterUserResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<RegisterUserResponse>> Register(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var result = await _authClient.RegisterAsync(request, cancellationToken);

        return Created(string.Empty, result);
    }
}
