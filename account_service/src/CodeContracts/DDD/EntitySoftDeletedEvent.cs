namespace CodeContracts.DDD;

public class EntitySoftDeletedEvent<T>(string id) : IDomainEvent where T : IEntity
{
    public string Message => $"{nameof(T)} with id {id} was successfully deleted.";
}