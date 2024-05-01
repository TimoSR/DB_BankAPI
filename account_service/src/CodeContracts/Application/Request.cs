namespace CodeContracts.Application;

public class Request : IRequest
{
    public Guid RequestId { get; init; } = new();
}