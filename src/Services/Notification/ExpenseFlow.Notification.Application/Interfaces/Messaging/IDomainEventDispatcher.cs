using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ExpenseFlow.Notification.Application.Interfaces.Messaging;

public interface IDomainEventDispatcher
{
    Task DispatchAndClearEventsAsync(object entity, CancellationToken cancellationToken = default);
    Task DispatchAndClearEventsAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
}
