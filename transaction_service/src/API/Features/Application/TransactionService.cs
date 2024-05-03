using API.Features.Domain;
using CodeContracts.Application.ServiceResultPattern;

namespace API.Features.Application;

public class TransactionService : ITransactionService
{
    private readonly ITransactionRepository _transactionRepository;
    private readonly ILogger<TransactionService> _logger; 

    public TransactionService( 
        ITransactionRepository transactionRepository,
        ILogger<TransactionService> logger)
    {
        _transactionRepository = transactionRepository;
        _logger = logger;
    }
    
    public async Task<ServiceResult<List<Transaction>>> GetAllTransactionsAsync()
    {
        try
        {
            var transactions = await _transactionRepository.GetTransactionsAsync();
            return ServiceResult<List<Transaction>>.Success(transactions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to get accounts.");
            return ServiceResult<List<Transaction>>.Failure("Failed to get accounts.");
        }
    }

    public async Task<ServiceResult<Transaction>> GetTransactionByIdAsync(string id)
    {
        try
        {
            var account = await _transactionRepository.GetTransactionAsync(id);
        
            return ServiceResult<Transaction>.Success(account);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Failed to get account with ID {id}", id);
            return ServiceResult<Transaction>.Failure($"Failed to get account with {id}");
        }
    }
    
    public async Task<ServiceResult> CreateTransactionAsync(CreateTransactionCommand command)
    {
        try
        {
            var account = new Transaction()
            {
                AccountId = command.AccountId,
                Amount = command.Amount
            };
            
            await _transactionRepository.CreateTransactionAsync(command.Id, account);
            
            _logger.LogInformation("Command {CommandId} successfully created an transaction!", command.Id);
            return ServiceResult.Success($"Account {account.Id} Created Successfully!");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Command {CommandId} failed to create transaction", command.Id);
            return ServiceResult.Failure("Failed to create account.");
        }
    }
}