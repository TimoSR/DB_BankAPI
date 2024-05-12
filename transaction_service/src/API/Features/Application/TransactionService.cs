using API.Features.Domain;
using CodeContracts.Application.ServiceResultPattern;
using CodeContracts.Infrastructure;

namespace API.Features.Application;

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly ITransactionRepository _repository;
    private readonly ITransactionFactory _factory;
    private readonly IDomainEventDispatcher _dispatcher;

    public TransactionService( 
        ITransactionRepository repository,
        ITransactionFactory factory,
        ILogger<TransactionService> logger,
        IDomainEventDispatcher dispatcher)
    {
        _repository = repository;
        _factory = factory;
        _logger = logger;
        _dispatcher = dispatcher;
    }
    
    public async Task<ServiceResult<List<TransactionDTO>>> GetAllTransactionsAsync()
    {
        try
        {
            var transactions = await _repository.GetTransactionsAsync();
            
            var transactionDTOs = new List<TransactionDTO>();
            
            foreach (var transaction in transactions)
            {
                transactionDTOs.Add(new TransactionDTO()
                {
                    Id = transaction.Id,
                    AccountId = transaction.AccountId,
                    Amount = transaction.Amount,
                    Time = transaction.Time
                });
            }
            
            return ServiceResult<List<TransactionDTO>>.Success(transactionDTOs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get accounts.");
            return ServiceResult<List<TransactionDTO>>.Failure("Failed to get accounts.");
        }
    }

    public async Task<ServiceResult<TransactionDTO>> GetTransactionByIdAsync(string id)
    {
        try
        {
            var transaction = await _repository.GetTransactionAsync(id);

            var transactionDTO = new TransactionDTO()
            {
                Id = transaction.Id,
                AccountId = transaction.AccountId,
                Amount = transaction.Amount,
                Time = transaction.Time
            };
        
            return ServiceResult<TransactionDTO>.Success(transactionDTO);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed to get account with ID {id}", id);
            return ServiceResult<TransactionDTO>.Failure($"Failed to get account with {id}");
        }
    }
    
    public async Task<ServiceResult> CreateTransactionAsync(CreateTransactionCommand command)
    {
        try
        {
            var transaction = _factory.CreateTransaction(command.Id, command.AccountId, command.Amount);
            
            await _repository.AddTransactionAsync(command.Id, transaction);
            
            await _dispatcher.DispatchEventsAsync(transaction);
            
            _logger.LogInformation("Command {CommandId} successfully created an transaction!", command.Id);
            return ServiceResult.Success($"Transaction {transaction.Id} Created Successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Command {CommandId} failed to create transaction", command.Id);
            return ServiceResult.Failure("Failed to create transaction.");
        }
    }
}