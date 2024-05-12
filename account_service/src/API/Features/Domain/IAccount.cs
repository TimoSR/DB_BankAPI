namespace API.Features.Domain;

public interface IAccount
{
    public string ID { get; set; }
    public string CPR { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; }
}