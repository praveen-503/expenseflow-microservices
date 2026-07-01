using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ExpenseFlow.Expense.Domain.Common;
using System;

namespace ExpenseFlow.Expense.Api.Common;

[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public abstract class ApiController : ControllerBase
{
    protected readonly ISender Sender;

    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected ActionResult HandleResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
        {
            return result.Value is bool b && b ? NoContent() : Ok(result.Value);
        }

        return MapErrorToActionResult(result.Error);
    }

    private ActionResult MapErrorToActionResult(Error error)
    {
        if (error.Code.Contains("NotFound", StringComparison.InvariantCultureIgnoreCase))
        {
            return NotFound(CreateProblemDetails(error, StatusCodes.Status404NotFound));
        }

        return BadRequest(CreateProblemDetails(error, StatusCodes.Status400BadRequest));
    }

    private ProblemDetails CreateProblemDetails(Error error, int statusCode)
    {
        return new ProblemDetails
        {
            Title = error.Code,
            Detail = error.Message,
            Status = statusCode,
            Type = $"https://httpstatuses.com/{statusCode}"
        };
    }
}
