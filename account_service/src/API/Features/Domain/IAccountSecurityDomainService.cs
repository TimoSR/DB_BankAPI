namespace API.Features.Domain;

public interface IAccountSecurityDomainService
{
    string Hash(string cpr);
}