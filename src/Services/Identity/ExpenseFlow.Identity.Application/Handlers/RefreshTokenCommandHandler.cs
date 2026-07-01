using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.DTOs;
using ExpenseFlow.Identity.Application.Interfaces;

namespace ExpenseFlow.Identity.Application.Handlers;

using ExpenseFlow.Identity.Application.Commands;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, Result<AuthResponseDto>>
{
    private readonly IAuthenticationService _authService;

    public RefreshTokenCommandHandler(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task<Result<AuthResponseDto>> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var dto = new RefreshTokenRequestDto(request.Token, request.RefreshToken);
        return await _authService.RefreshTokenAsync(dto, cancellationToken);
    }
}
