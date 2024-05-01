namespace CodeContracts.DDD;

public class Entity : IEntity
{
    public string Id { get; protected init; } = new Guid().ToString();
}