namespace API.Features.Domain;

public class AccountFactory : IAccountFactory
{
    public Account CreateAccount(
        Guid commandId,
        string cpr, 
        string firstName, 
        string lastName)
    {
        var id = Guid.NewGuid().ToString();
        
        var account = new Account { ID = id, CPR = cpr, FirstName = firstName, LastName = lastName };
        
        account.InitializeAccount(commandId);

        return account;
    }
}