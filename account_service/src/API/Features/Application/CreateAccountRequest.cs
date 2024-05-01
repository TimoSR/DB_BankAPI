using API.Features.Domain;
using CodeContracts.Application;

namespace API.Features.Application;

public class CreateAccountRequest : Request, IAccount
{
    public string CPR { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; }
}