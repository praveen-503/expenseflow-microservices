using System;
using System.Collections.Generic;
using ExpenseFlow.Notification.Domain.DomainEvents;

namespace ExpenseFlow.Notification.Domain.Entities;

public abstract class BaseEntity<TId>
{
    public TId Id { get; protected set; } = default!;
    
    private readonly List<IDomainEvent> _domainEvents = new();
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
    public void RemoveDomainEvent(IDomainEvent domainEvent) => _domainEvents.Remove(domainEvent);
    public void ClearDomainEvents() => _domainEvents.Clear();
}
