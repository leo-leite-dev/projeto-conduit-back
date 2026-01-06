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

    [HttpPost("users/login")]
    [AllowAnonymous]
    [ProducesResponseType(typeof(LoginUserResponse), StatusCodes.Status200OK)]
    public async Task<ActionResult<LoginUserResponse>> Login(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken
    )
    {
        var authResult = await _authClient.LoginAsync(request, cancellationToken);

        var response = new LoginUserResponse(
            new UserResponse(
                Email: request.User.Email,
                Username: string.Empty,
                Token: authResult.AccessToken
            )
        );

        return Ok(response);
    }
}
