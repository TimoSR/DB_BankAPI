using CodeContracts.DDD;

namespace API.Features.CreateAccount.Domain;

public interface ISavingsAccount : IAggregateRoot
{
    public string CPR { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}