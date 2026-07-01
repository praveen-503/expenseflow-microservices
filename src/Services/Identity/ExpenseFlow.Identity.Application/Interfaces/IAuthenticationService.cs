using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.DTOs;

namespace ExpenseFlow.Identity.Application.Interfaces;

public interface IAuthenticationService
{
    Task<Result<AuthResponseDto>> LoginAsync(LoginRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponseDto>> RegisterAsync(RegisterRequestDto request, CancellationToken cancellationToken = default);
    Task<Result<AuthResponseDto>> RefreshTokenAsync(RefreshTokenRequestDto request, CancellationToken cancellationToken = default);
}
