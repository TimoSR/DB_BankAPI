using CodeContracts.DDD;
using CodeContracts.Infrastructure;
using MediatR;

namespace Infrastructure.Persistence;

public class DomainEventDispatcher(IPublisher mediator) : IDomainEventDispatcher
{
    public async Task DispatchEventsAsync<T>(T entity) where T : AggregateRoot
    {
        var domainEvents = entity.DomainEvents?.ToList() ?? new List<INotification>();

        foreach (var domainEvent in domainEvents)
        {
            await mediator.Publish(domainEvent);
        }

        entity.ClearDomainEvents();
    }
}