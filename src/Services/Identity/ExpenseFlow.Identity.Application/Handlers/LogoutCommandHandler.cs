using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.Interfaces;

namespace ExpenseFlow.Identity.Application.Handlers;

using ExpenseFlow.Identity.Application.Commands;

public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result<bool>>
{
    private readonly IAuthenticationService _authService;

    public LogoutCommandHandler(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task<Result<bool>> Handle(LogoutCommand request, CancellationToken cancellationToken)
    {
        return await _authService.LogoutAsync(request.RefreshToken, cancellationToken);
    }
}
