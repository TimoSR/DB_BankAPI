namespace CodeContracts.Application;

public interface IIdempotency
{
    Guid Id { get; init; }
}