using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Expense.Application.Common.Messaging;

namespace ExpenseFlow.Expense.Application.Interfaces.Messaging;

public interface IEventConsumer<in TEvent> where TEvent : BaseIntegrationEvent
{
    Task ConsumeAsync(TEvent @event, CancellationToken cancellationToken = default);
}
