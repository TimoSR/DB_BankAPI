using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace CodeContracts.DDD;

public abstract class AggregateRoot : IAggregateRoot
{
    // Auto generates the guid as default, but custom ID can be set
    public string ID { get; set; }

    [NotMapped] public List<INotification>? DomainEvents { get; private set; }

    public bool IsDeleted { get; private set; }

    public virtual void MarkAsDeleted<T>() where T : IEntity
    {
        IsDeleted = true;
        AddDomainEvent(new EntitySoftDeletedEvent<T>(ID));
    }

    public void TriggerDeleteNotification<T>() where T : IEntity
    {
        AddDomainEvent(new EntityDeletedEvent<T>(ID));
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