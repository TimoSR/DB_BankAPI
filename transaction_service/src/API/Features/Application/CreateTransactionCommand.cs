using System.ComponentModel.DataAnnotations;
using API.Features.Domain;
using CodeContracts.Application;

namespace API.Features.Application;

public class CreateTransactionCommand : Command
{
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
}