using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.DTOs;
using ExpenseFlow.Identity.Application.Interfaces;

namespace ExpenseFlow.Identity.Application.Handlers;

using ExpenseFlow.Identity.Application.Commands;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<AuthResponseDto>>
{
    private readonly IAuthenticationService _authService;

    public LoginCommandHandler(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task<Result<AuthResponseDto>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var dto = new LoginRequestDto(request.Email, request.Password);
        return await _authService.LoginAsync(dto, cancellationToken);
    }
}
