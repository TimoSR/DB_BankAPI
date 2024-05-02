namespace CodeContracts.DDD;

public class Entity : IEntity
{
    public string Id { get; set; } = Guid.NewGuid().ToString();
}