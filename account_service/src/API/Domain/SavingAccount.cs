namespace API.Domain;

public class SavingsAccount
{
    public int AccountId { get; set; }
    public string CPR { get; set; }
    public string Name { get; set; }
    public decimal Balance { get; set; }
}