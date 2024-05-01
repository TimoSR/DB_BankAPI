using CodeContracts.DDD;

namespace CodeContracts.Infrastructure;

public interface IDomainEventDispatcher
{
    Task DispatchEventsAsync<T>(T entity) where T : AggregateRoot;
}