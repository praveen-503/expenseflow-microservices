using System;

namespace ExpenseFlow.Identity.Application.DTOs;

public record AuthResponseDto(string Token, string RefreshToken, DateTime TokenExpiry, UserDto User);
