namespace rest_with_asp_net_10_cviana.Auth.Contract
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool Verify(string rawPassword, string hashedPassword);
    }
}
