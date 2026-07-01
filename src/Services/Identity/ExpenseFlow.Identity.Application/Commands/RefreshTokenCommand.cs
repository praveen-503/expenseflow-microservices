using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.DTOs;

namespace ExpenseFlow.Identity.Application.Commands;

public record RefreshTokenCommand(string Token, string RefreshToken) : IRequest<Result<AuthResponseDto>>;
