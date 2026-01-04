using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Conduit.Api.Security.Authentication;

namespace Conduit.Api.Controllers;

[ApiController]
[Authorize]
[Route("user")]
public sealed class UserController : ControllerBase
{
    [HttpGet]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            user = new
            {
                email = User.GetEmail(),
                username = User.GetUsername(),
                token = Request.Headers.Authorization.ToString().Replace("Bearer ", ""),
                bio = string.Empty,
                image = string.Empty
            }
        });
    }
}
