using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Common.Messaging;

namespace ExpenseFlow.Notification.Application.Interfaces.Messaging;

public interface IEventConsumer<in TEvent> where TEvent : BaseIntegrationEvent
{
    Task ConsumeAsync(TEvent @event, CancellationToken cancellationToken = default);
}
