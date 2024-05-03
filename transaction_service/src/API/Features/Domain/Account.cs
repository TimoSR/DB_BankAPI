using CodeContracts.DDD;

namespace API.Features.Domain;

public class Account : AggregateRoot, IAccount
{
    public string CPR { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; private set; }
    
    public void CreateAccount(Guid requestId)
    {
        if (string.IsNullOrEmpty(CPR) || string.IsNullOrEmpty(FirstName) || string.IsNullOrEmpty(LastName))
            throw new InvalidOperationException("Cannot create account because one or more required properties are not set.");
        
        AddDomainEvent(new AccountCreatedEvent(requestId, Id, DateTime.Now));
    }

    public void UpdateBalance(Guid requestId, decimal amount)
    {
        if (amount.Equals(0))
            throw new InvalidOperationException("Cannot update account balance as the value 0 is not a valid input.");
        
        Balance += amount;
        
        AddDomainEvent(new BalanceUpdatedEvent(requestId, Id, amount, DateTime.Now));
    }
}