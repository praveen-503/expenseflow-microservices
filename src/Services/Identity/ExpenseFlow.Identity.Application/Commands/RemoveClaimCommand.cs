using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using System;

namespace ExpenseFlow.Identity.Application.Commands;

public record RemoveClaimCommand(Guid UserId, string ClaimType, string ClaimValue) : IRequest<Result<bool>>;
