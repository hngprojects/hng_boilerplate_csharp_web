using Microsoft.AspNetCore.Mvc;

namespace Hng.Web.Extensions;

public static class ControllerSuccessResponseExtensions
{
    public static ObjectResult CreatedResult(this ControllerBase controller, string message, object data)
    {
        return new ObjectResult(new
        {
            status = "success",
            message,
            data
        })
        {
            StatusCode = StatusCodes.Status201Created
        };
    }
}


