using System;

namespace ExpenseFlow.Identity.Application.DTOs;

public record UserDto(Guid Id, string Email, string FirstName, string LastName);
