using Conduit.Api.Authentication;
using Conduit.Api.Authentication.Contracts.Auth.Login;
using Conduit.Api.Authentication.Contracts.Auth.Register;
using Conduit.Api.Contracts.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Controllers;

[ApiController]
public sealed class AuthController : ControllerBase
{
    private readonly AuthServiceClient _authClient;

    public AuthController(AuthServiceClient authClient)
    {
        _authClient = authClient;
    }

    [HttpPost("/users")]
    [AllowAnonymous]
    public async Task<IActionResult> RegisterUsers(
        [FromBody] RegisterUserRequest request,
        CancellationToken ct
    )
    {
        if (request?.User is null)
            return BadRequest("Invalid payload");

        var result = await _authClient.RegisterAsync(request, ct);

        return Created(string.Empty, UserMapper.FromRegister(request, result));
    }

    [HttpPost("/users/login")]
    [AllowAnonymous]
    public async Task<IActionResult> LoginUsers(
        [FromBody] LoginUserRequest request,
        CancellationToken ct
    )
    {
        var result = await _authClient.LoginAsync(request, ct);
        return Ok(UserMapper.FromLogin(request, result));
    }

    [HttpPost("/users/refresh")]
    [AllowAnonymous]
    public async Task<IActionResult> Refresh(CancellationToken ct)
    {
        var refreshToken = Request.GetRefreshToken();
        var result = await _authClient.RefreshAsync(refreshToken, ct);

        return Ok(new { user = new { token = result.AccessToken } });
    }
}
