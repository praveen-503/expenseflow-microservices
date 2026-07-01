using System;
using MediatR;
using ExpenseFlow.Identity.Domain.Common;

namespace ExpenseFlow.Identity.Application.Commands;

public record ChangePasswordCommand(Guid UserId, string CurrentPassword, string NewPassword) : IRequest<Result<bool>>;
