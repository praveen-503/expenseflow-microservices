using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExpenseFlow.Notification.Application.Interfaces.Messaging;
using ExpenseFlow.Notification.Domain.Entities;
using ExpenseFlow.Notification.Application.Common;

namespace ExpenseFlow.Notification.Persistence.Repositories;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IMediator _mediator;

    public DomainEventDispatcher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task DispatchAndClearEventsAsync(object entity, CancellationToken cancellationToken = default)
    {
        if (entity is IBaseEntity baseEntity)
        {
            var events = baseEntity.DomainEvents.ToList();
            baseEntity.ClearDomainEvents();

            foreach (var domainEvent in events)
            {
                var type = typeof(DomainEventNotification<>).MakeGenericType(domainEvent.GetType());
                var notification = Activator.CreateInstance(type, domainEvent);
                
                if (notification is INotification mediatrNotification)
                {
                    await _mediator.Publish(mediatrNotification, cancellationToken);
                }
            }
        }
    }

    public async Task DispatchAndClearEventsAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default)
    {
        foreach (var entity in entities)
        {
            await DispatchAndClearEventsAsync(entity, cancellationToken);
        }
    }
}
