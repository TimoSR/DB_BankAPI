using CodeContracts.Application;

namespace API.Features.Application;

public class UpdateBalanceRequest : Request
{
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
}