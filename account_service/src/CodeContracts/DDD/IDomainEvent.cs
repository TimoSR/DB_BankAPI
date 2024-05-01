using MediatR;

namespace CodeContracts.DDD;

public interface IDomainEvent : INotification
{
    public string Message { get; }
}