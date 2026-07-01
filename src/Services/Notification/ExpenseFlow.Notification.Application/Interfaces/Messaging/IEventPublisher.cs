using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Common.Messaging;

namespace ExpenseFlow.Notification.Application.Interfaces.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : BaseIntegrationEvent;
}
