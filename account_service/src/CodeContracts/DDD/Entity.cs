namespace CodeContracts.DDD;

public class Entity : IEntity
{
    public string ID { get; set; } = Guid.NewGuid().ToString();
}