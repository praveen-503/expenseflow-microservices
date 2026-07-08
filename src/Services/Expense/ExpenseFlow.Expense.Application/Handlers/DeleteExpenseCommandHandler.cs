using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using ExpenseFlow.Expense.Domain.Common;
using ExpenseFlow.Expense.Domain.Interfaces;
using ExpenseFlow.Expense.Application.Commands;
using ExpenseFlow.Expense.Application.Interfaces.Messaging;
using ExpenseFlow.Expense.Application.Common.Messaging.Events;

namespace ExpenseFlow.Expense.Application.Handlers;

public class DeleteExpenseCommandHandler : IRequestHandler<DeleteExpenseCommand, Result<bool>>
{
    private readonly IExpenseRepository _expenseRepository;
    private readonly IEventPublisher _eventPublisher;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteExpenseCommandHandler(
        IExpenseRepository expenseRepository,
        IEventPublisher eventPublisher,
        IUnitOfWork unitOfWork)
    {
        _expenseRepository = expenseRepository;
        _eventPublisher = eventPublisher;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<bool>> Handle(DeleteExpenseCommand request, CancellationToken cancellationToken)
    {
        var expense = await _expenseRepository.GetByIdAsync(request.Id, cancellationToken);
        if (expense == null || expense.UserId != request.UserId)
        {
            return Result<bool>.Failure(new Error("Expense.NotFound", "Expense not found."));
        }

        _expenseRepository.Delete(expense);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        // Publish integration event
        var integrationEvent = new ExpenseDeletedIntegrationEvent(expense.Id, expense.UserId);
        await _eventPublisher.PublishAsync(integrationEvent, cancellationToken);

        return Result<bool>.Success(true);
    }
}
