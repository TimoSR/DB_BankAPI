namespace API.Features.Application;

public record TransactionDTO
{
    public required string Id { get; set; }
    public required string AccountId { get; set; }
    public required decimal Amount { get; set; }
    public required DateTime Time { get; set; }
}