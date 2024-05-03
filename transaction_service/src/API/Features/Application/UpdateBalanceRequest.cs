using System.ComponentModel.DataAnnotations;
using CodeContracts.Application;

namespace API.Features.Application;

public class UpdateBalanceRequest : Request
{
    [Required(ErrorMessage = "Account ID is required.")]
    public string AccountId { get; set; }
    
    [Required(ErrorMessage = "Amount is required.")]
    [NonZeroDecimal]
    public decimal Amount { get; set; }
}