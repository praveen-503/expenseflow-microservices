using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.DTOs;

namespace ExpenseFlow.Identity.Application.Commands;

public record RegisterCommand(string Email, string Password, string FirstName, string LastName) : IRequest<Result<AuthResponseDto>>;
