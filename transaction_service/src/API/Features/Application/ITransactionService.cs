using API.Features.Domain;
using CodeContracts.Application.ServiceResultPattern;

namespace API.Features.Application;

public interface ITransactionService
{
    Task<ServiceResult<List<TransactionDTO>>> GetAllTransactionsAsync();
    Task<ServiceResult<TransactionDTO>> GetTransactionByIdAsync(string id);
    Task<ServiceResult> CreateTransactionAsync(CreateTransactionCommand command);
    Task<ServiceResult<List<TransactionDTO>>> GetLast10TransactionsAsync(string id);
}