using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using System;

namespace ExpenseFlow.Identity.Application.Commands;

public record CreateRoleCommand(string Name) : IRequest<Result<Guid>>;
