using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Identity.Application.Common.Messaging;

namespace ExpenseFlow.Identity.Application.Interfaces.Messaging;

public interface IEventConsumer<in TEvent> where TEvent : BaseIntegrationEvent
{
    Task ConsumeAsync(TEvent @event, CancellationToken cancellationToken = default);
}
