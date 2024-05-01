namespace API.Features.CreateAccount.Domain;

public interface IAccountSecurityDomainService
{
    string Hash(string cpr);
}