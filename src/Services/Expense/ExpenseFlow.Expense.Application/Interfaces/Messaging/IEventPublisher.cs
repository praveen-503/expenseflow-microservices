using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Expense.Application.Common.Messaging;

namespace ExpenseFlow.Expense.Application.Interfaces.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : BaseIntegrationEvent;
}
