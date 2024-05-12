namespace API.Features.Domain;

public interface IAccountFactory
{
    Account CreateAccount(
        Guid commandId,
        string cpr,
        string firstName,
        string lastName);
}