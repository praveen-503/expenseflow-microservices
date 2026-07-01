namespace ExpenseFlow.Identity.Application.DTOs;

public record RegisterRequestDto(string Email, string Password, string FirstName, string LastName);
