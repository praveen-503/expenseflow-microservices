using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using System;

namespace ExpenseFlow.Identity.Application.Commands;

public record AssignRoleCommand(Guid UserId, string RoleName) : IRequest<Result<bool>>;
