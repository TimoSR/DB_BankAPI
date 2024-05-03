using API.Features.Domain;

namespace API.Features.Application;

public record AccountDTO
{
    public string Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public decimal Balance { get; set; }
}