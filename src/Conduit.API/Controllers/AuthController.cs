using Conduit.Api.Authentication;
using Conduit.Api.Contracts.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers;

[ApiController]
[Route("")]
public sealed class AuthController : ControllerBase
{
    private readonly AuthServiceClient _authClient;

    public AuthController(AuthServiceClient authClient)
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
        var authResult = await _authClient.RegisterAsync(request, cancellationToken);

        var response = new RegisterUserResponse(
            new UserResponse(
                Email: request.User.Email,
                Username: request.User.Username,
                Token: authResult.AccessToken
            )
        );

        return Created(string.Empty, response);
    }
}
