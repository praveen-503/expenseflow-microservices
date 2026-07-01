using MediatR;
using ExpenseFlow.Identity.Domain.Common;

namespace ExpenseFlow.Identity.Application.Commands;

public record LogoutCommand(string RefreshToken) : IRequest<Result<bool>>;
