using CodeContracts.DDD;

namespace API.Features.Domain;

public class Account : AggregateRoot, IAccount
{
    public string CPR { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; init; }
    
    public void Initialize()
    {
        // Potentially additional initialization logic here
        AddDomainEvent(new AccountCreatedEvent(Id, DateTime.Now));
    }
}