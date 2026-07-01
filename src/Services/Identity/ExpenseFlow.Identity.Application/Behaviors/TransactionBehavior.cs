using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using ExpenseFlow.Identity.Domain.Interfaces;

namespace ExpenseFlow.Identity.Application.Behaviors;

public class TransactionBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly ILogger<TransactionBehavior<TRequest, TResponse>> _logger;
    private readonly IUnitOfWork _unitOfWork;

    public TransactionBehavior(ILogger<TransactionBehavior<TRequest, TResponse>> logger, IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;

        // Apply transaction only to Commands
        if (!requestName.EndsWith("Command"))
        {
            return await next();
        }

        _logger.LogInformation("Begin transaction for {RequestName}", requestName);

        var response = await next();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Committed transaction for {RequestName}", requestName);

        return response;
    }
}
