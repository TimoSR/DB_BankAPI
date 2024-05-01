using API.Features.CreateAccount.Domain;

namespace API.Features.Application;

public record AccountDto : IAccount
{
    public string Id { get; set; }
    public string CPR { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
}