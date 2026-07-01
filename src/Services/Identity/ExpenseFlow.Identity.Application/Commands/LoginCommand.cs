using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.DTOs;

namespace ExpenseFlow.Identity.Application.Commands;

public record LoginCommand(string Email, string Password) : IRequest<Result<AuthResponseDto>>;
