using Org.BouncyCastle.Utilities;
using rest_with_asp_net_10_cviana.Auth.Contract;
using System.Security.Cryptography;
using System.Text;

namespace rest_with_asp_net_10_cviana.Auth.Tools
{
    public class Sha256PasswordHasher : IPasswordHasher
    {
        public string Hash(string password)
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashedBytes = SHA256.HashData(inputBytes);
            StringBuilder builder = new();
            foreach(byte b in hashedBytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        public bool Verify(string rawPassword, string hashedPassword)
        {
            return Hash(rawPassword) == hashedPassword;
        }
    }
}
