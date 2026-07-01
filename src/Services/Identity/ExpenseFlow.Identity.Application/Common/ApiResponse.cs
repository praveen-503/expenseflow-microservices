using System;

namespace ExpenseFlow.Identity.Application.Common;

public class ApiResponse<T>
{
    public ApiResponse(T data, string? message = null)
    {
        IsSuccess = true;
        Data = data;
        Message = message;
    }

    public ApiResponse(string error, string? message = null)
    {
        IsSuccess = false;
        Error = error;
        Message = message;
    }

    public bool IsSuccess { get; }
    public T? Data { get; }
    public string? Error { get; }
    public string? Message { get; }
    public DateTime Timestamp { get; } = DateTime.UtcNow;
}
