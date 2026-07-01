using System.Collections.Generic;
using System.Security.Claims;
using ExpenseFlow.Identity.Domain.Entities;

namespace ExpenseFlow.Identity.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user, IEnumerable<string> roles, IEnumerable<Claim> claims);
    string GenerateRefreshToken();
}
