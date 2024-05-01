using CodeContracts.DDD;

namespace API.Features.CreateAccount.Domain;

public interface IAccount
{
    public string CPR { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; }
}