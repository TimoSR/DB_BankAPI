using CodeContracts.Application.ServiceResultPattern;

namespace CodeContracts.Application;

public interface ICommandHandler<TCommand> where TCommand : ICommand
{
    Task<ServiceResult> Handle(TCommand command);
}

// Or, if you want to return a data payload with the result:
public interface ICommandHandler<TCommand, TResult> where TCommand : ICommand
{
    Task<ServiceResult<TResult>> Handle(TCommand command);
}
