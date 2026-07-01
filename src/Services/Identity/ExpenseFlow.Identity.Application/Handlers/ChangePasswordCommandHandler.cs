using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.Interfaces;

namespace ExpenseFlow.Identity.Application.Handlers;

using ExpenseFlow.Identity.Application.Commands;

public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<bool>>
{
    private readonly IAuthenticationService _authService;

    public ChangePasswordCommandHandler(IAuthenticationService authService)
    {
        _authService = authService;
    }

    public async Task<Result<bool>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        return await _authService.ChangePasswordAsync(request.UserId, request.CurrentPassword, request.NewPassword, cancellationToken);
    }
}
