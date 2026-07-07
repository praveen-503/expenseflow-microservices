using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Identity.Domain.Common;
using ExpenseFlow.Identity.Application.DTOs;
using ExpenseFlow.Identity.Application.Interfaces;
using ExpenseFlow.Identity.Application.Interfaces.Messaging;
using ExpenseFlow.Identity.Application.Common.Messaging;

namespace ExpenseFlow.Identity.Application.Handlers;

using ExpenseFlow.Identity.Application.Commands;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<AuthResponseDto>>
{
    private readonly IAuthenticationService _authService;
    private readonly IEventPublisher _eventPublisher;

    public RegisterCommandHandler(IAuthenticationService authService, IEventPublisher eventPublisher)
    {
        _authService = authService;
        _eventPublisher = eventPublisher;
    }

    public async Task<Result<AuthResponseDto>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var dto = new RegisterRequestDto(request.Email, request.Password, request.FirstName, request.LastName);
        var result = await _authService.RegisterAsync(dto, cancellationToken);

        //if (result.IsSuccess && result.Value != null)
        //{
        //    var user = result.Value.User;
        //    var integrationEvent = new UserRegisteredIntegrationEvent
        //    {
        //        UserId = user.Id,
        //        Email = user.Email,
        //        FirstName = user.FirstName,
        //        LastName = user.LastName
        //    };

        //    await _eventPublisher.PublishAsync(integrationEvent, cancellationToken);
        //}

        return result;
    }
}
