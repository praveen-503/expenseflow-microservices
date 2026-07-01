using System;

namespace ExpenseFlow.Identity.Application.Interfaces;

public interface IDateTimeProvider
{
    DateTime UtcNow { get; }
}
