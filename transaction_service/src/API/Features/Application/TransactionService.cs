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
    
    public async Task<ServiceResult<List<TransactionDTO>>> GetAllTransactionsAsync()
    {
        try
        {
            var transactions = await _transactionRepository.GetTransactionsAsync();
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
            var transaction = await _transactionRepository.GetTransactionAsync(id);

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