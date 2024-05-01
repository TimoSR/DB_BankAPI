using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace CodeContracts.DDD;

public abstract class AggregateRoot : IAggregateRoot
{
    public string Id { get; set; } = new Guid().ToString();

    [NotMapped] public List<INotification>? DomainEvents { get; private set; }

    public bool IsDeleted { get; private set; }

    public virtual void MarkAsDeleted<T>() where T : IEntity
    {
        IsDeleted = true;
        AddDomainEvent(new EntitySoftDeletedEvent<T>(Id));
    }

    public void TriggerDeleteNotification<T>() where T : IEntity
    {
        AddDomainEvent(new EntityDeletedEvent<T>(Id));
    }

    public void AddDomainEvent(INotification eventItem)
    {
        DomainEvents ??= new List<INotification>();
        DomainEvents?.Add(eventItem);
    }

    public void RemoveDomainEvent(INotification eventItem)
    {
        DomainEvents?.Remove(eventItem);
    }

    public void ClearDomainEvents()
    {
        DomainEvents?.Clear();
    }
}