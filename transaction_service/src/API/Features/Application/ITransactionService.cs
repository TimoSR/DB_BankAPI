using API.Features.Domain;
using CodeContracts.Application.ServiceResultPattern;

namespace API.Features.Application;

public interface ITransactionService
{
    Task<ServiceResult<List<Transaction>>> GetAllTransactionsAsync();
    Task<ServiceResult<Transaction>> GetTransactionByIdAsync(string id);
    Task<ServiceResult> CreateTransactionAsync(CreateTransactionCommand command);
}