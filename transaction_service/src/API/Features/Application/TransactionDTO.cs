namespace API.Features.Application;

public class TransactionDTO
{
    public string Id { get; set; }
    public string AccountId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Time { get; set; }
}