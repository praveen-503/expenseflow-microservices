using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Identity.Application.Common.Messaging;

namespace ExpenseFlow.Identity.Application.Interfaces.Messaging;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) 
        where TEvent : BaseIntegrationEvent;
}
