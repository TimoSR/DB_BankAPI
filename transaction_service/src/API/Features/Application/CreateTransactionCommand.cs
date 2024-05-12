using System.ComponentModel.DataAnnotations;
using API.Features.Domain;
using CodeContracts.Application;

namespace API.Features.Application;

public record CreateTransactionCommand : Command
{
    public required string AccountId { get; set; }
    public required decimal Amount { get; set; }
}