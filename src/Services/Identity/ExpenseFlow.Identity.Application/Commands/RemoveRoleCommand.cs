using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using System;

namespace ExpenseFlow.Identity.Application.Commands;

public record RemoveRoleCommand(Guid UserId, string RoleName) : IRequest<Result<bool>>;
