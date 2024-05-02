using System.ComponentModel.DataAnnotations.Schema;
using MediatR;

namespace CodeContracts.DDD;

public interface IAggregateRoot : IEntity
{
    [NotMapped] List<INotification> DomainEvents { get; }
    void AddDomainEvent(INotification eventItem);
    void RemoveDomainEvent(INotification eventItem);
    void ClearDomainEvents();
}