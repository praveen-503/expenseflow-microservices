using System;

namespace ExpenseFlow.Notification.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string name, object key) 
        : base($"Entity \"{name}\" ({key}) was not found.") { }
}

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}

public class ConflictException : Exception
{
    public ConflictException(string message) : base(message) { }
}

public class ForbiddenException : Exception
{
    public ForbiddenException() : base("Access to this resource is forbidden.") { }
}

public class UnauthorizedException : Exception
{
    public UnauthorizedException() : base("User is unauthorized.") { }
}
