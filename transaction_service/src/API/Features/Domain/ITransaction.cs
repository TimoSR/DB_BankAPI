namespace API.Features.Domain;

public interface ITransaction
{
    public string AccountId { get; set; }
    public decimal Amount { get; set;  }
    public DateTime Time { get; }
}