using System.ComponentModel.DataAnnotations;
using CodeContracts.Application;

namespace API.Features.Application;

public record CreateTransactionCommand : Command
{
    [Required(ErrorMessage = "Account ID is required.")]
    public required string AccountId { get; set; }
    
    [Required(ErrorMessage = "Amount is required.")]
    [NonZeroDecimal]
    public required decimal Amount { get; set; }
}