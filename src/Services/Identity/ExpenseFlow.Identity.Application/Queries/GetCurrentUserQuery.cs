using System;
using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.DTOs;

namespace ExpenseFlow.Identity.Application.Queries;

public record GetCurrentUserQuery : IRequest<Result<UserDto>>;
