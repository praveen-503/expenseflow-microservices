using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Exceptions;

namespace ExpenseFlow.Notification.Api.Middleware;

public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "ExpenseFlow exception handled: {Message}", exception.Message);

        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Title = "Server Error",
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Detail = "An unexpected error occurred."
        };

        if (exception is ValidationException validationException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Validation Error";
            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
            problemDetails.Detail = validationException.Message;
            problemDetails.Extensions["errors"] = validationException.Errors;
        }
        else if (exception is NotFoundException notFoundException)
        {
            problemDetails.Status = StatusCodes.Status404NotFound;
            problemDetails.Title = "Not Found";
            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.4";
            problemDetails.Detail = notFoundException.Message;
        }
        else if (exception is BadRequestException badRequestException)
        {
            problemDetails.Status = StatusCodes.Status400BadRequest;
            problemDetails.Title = "Bad Request";
            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1";
            problemDetails.Detail = badRequestException.Message;
        }
        else if (exception is ConflictException conflictException)
        {
            problemDetails.Status = StatusCodes.Status409Conflict;
            problemDetails.Title = "Conflict";
            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8";
            problemDetails.Detail = conflictException.Message;
        }
        else if (exception is ForbiddenException forbiddenException)
        {
            problemDetails.Status = StatusCodes.Status403Forbidden;
            problemDetails.Title = "Forbidden";
            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.3";
            problemDetails.Detail = forbiddenException.Message;
        }
        else if (exception is UnauthorizedException unauthorizedException)
        {
            problemDetails.Status = StatusCodes.Status401Unauthorized;
            problemDetails.Title = "Unauthorized";
            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
            problemDetails.Detail = unauthorizedException.Message;
        }
        else if (exception is UnauthorizedAccessException)
        {
            problemDetails.Status = StatusCodes.Status401Unauthorized;
            problemDetails.Title = "Unauthorized";
            problemDetails.Type = "https://datatracker.ietf.org/doc/html/rfc7235#section-3.1";
            problemDetails.Detail = "You are not authorized to access this resource.";
        }

        httpContext.Response.StatusCode = problemDetails.Status.Value;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}
