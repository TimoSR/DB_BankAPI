using System.Net;
using API.Features.Domain;
using CodeContracts.Application.ServiceResultPattern;

namespace API.Features.Application;

public class TransactionService : ITransactionService
{
    private readonly ILogger<TransactionService> _logger;
    private readonly ITransactionRepository _repository;
    private readonly ITransactionFactory _factory;
    private readonly IHttpClientFactory _httpClientFactory;

    public TransactionService( 
        ITransactionRepository repository,
        ITransactionFactory factory,
        IHttpClientFactory httpClientFactory,
        ILogger<TransactionService> logger)
    {
        _repository = repository;
        _factory = factory;
        _httpClientFactory = httpClientFactory;
        _logger = logger;
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
            _logger.LogError(ex, "Failed to get transactions.");
            return ServiceResult<List<TransactionDTO>>.Failure("Failed to get transactions.");
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
            _logger.LogError(ex,"Failed to get transaction with ID {id}", id);
            return ServiceResult<TransactionDTO>.Failure($"Failed to get transaction with {id}");
        }
    }
    
    public async Task<ServiceResult> CreateTransactionAsync(CreateTransactionCommand command)
    {
        try
        {
            // Create HTTP client from the factory
            var client = _httpClientFactory.CreateClient("AccountServiceClient");

            // Build the request URL with the account ID
            var requestUrl = $"https://localhost:7101/api/Account/GetById?id={command.AccountId}";

            // Send the GET request
            var response = await client.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NoContent)
            {
                
                _logger.LogInformation("{response}", response.IsSuccessStatusCode);
                
                var transaction = _factory.CreateTransaction(command.Id, command.AccountId, command.Amount);
            
                await _repository.AddTransactionAsync(command.Id, transaction);
            
                _logger.LogInformation("Command {CommandId} successfully created an transaction!", command.Id);
                return ServiceResult.Success($"Transaction {transaction.Id} Created Successfully!");
            }

            _logger.LogError("Failed to retrieve account with ID {AccountId}. Status code: {StatusCode}", command.AccountId, response.StatusCode);
            return ServiceResult.Failure($"Error: {response.StatusCode}");
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"Command {CommandId} failed to create transaction", command.Id);
            return ServiceResult.Failure("Failed to create transaction.");
        }
    }

    public async Task<ServiceResult<List<TransactionDTO>>> GetLast10AccountTransactionsAsync(string id)
    {
        try
        {
            var transactions = await _repository.GetLast10AccountTransactionsAsync(id);

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
            _logger.LogError(ex, "Failed to get transactions.");
            return ServiceResult<List<TransactionDTO>>.Failure("Failed to get transactions.");
        }
    }
}