using Conduit.Application.Abstractions.Results;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Api.Extensions;

public static class ResultToActionResultExtensions
{
    public static IActionResult ToActionResult<T>(this Result<T> result, ControllerBase controller)
    {
        if (result.IsSuccess)
            return controller.Ok(result.Value);

        return MapError(result.Error, controller);
    }

    private static IActionResult MapError(Error error, ControllerBase controller)
    {
        return error.Code switch
        {
            "NotFound" => controller.NotFound(new { error = error.Message }),
            "Unauthorized" => controller.Unauthorized(new { error = error.Message }),
            "Forbidden" => controller.Forbid(),
            "Validation" => controller.BadRequest(new { error = error.Message }),
            _ => controller.BadRequest(new { error = error.Message }),
        };
    }
}
