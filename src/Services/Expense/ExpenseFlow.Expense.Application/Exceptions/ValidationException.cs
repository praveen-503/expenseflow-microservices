using System;
using System.Collections.Generic;

namespace ExpenseFlow.Expense.Application.Exceptions;

public class ValidationException : Exception
{
    public ValidationException(IDictionary<string, string[]> failures)
        : base("One or more validation failures have occurred.")
    {
        Errors = failures;
    }

    public IDictionary<string, string[]> Errors { get; }
}
