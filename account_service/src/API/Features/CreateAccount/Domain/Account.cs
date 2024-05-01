using CodeContracts.DDD;

namespace API.Features.CreateAccount.Domain;

public class Account : AggregateRoot, IAccount
{
    public string CPR { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}