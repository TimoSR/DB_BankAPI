using System.Security.Cryptography;
using System.Text;

namespace API.Features.Domain;

public class AccountSecurityService : IAccountSecurityDomainService
{
    public string Hash(string cpr)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(cpr));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}