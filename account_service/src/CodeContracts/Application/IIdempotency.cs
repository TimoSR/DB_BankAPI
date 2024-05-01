namespace CodeContracts.Application;

public interface IIdempotency
{
    Guid RequestId { get; init; }
}