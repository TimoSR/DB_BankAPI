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
        if (string.IsNullOrEmpty(CPR) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            throw new InvalidOperationException("Cannot initialize account because one or more required properties are not set.");
        
        AddDomainEvent(new AccountCreatedEvent(Id, DateTime.Now));
    }
}