using System.Security.Cryptography;
using System.Text;

public static class SecurityHelpers
{
    public static string HashCPR(string cpr)
    {
        using (var sha256 = SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(cpr));
            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLowerInvariant();
        }
    }
}