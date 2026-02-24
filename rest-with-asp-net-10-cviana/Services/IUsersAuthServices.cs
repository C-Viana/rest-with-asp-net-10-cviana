using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.Services
{
    public interface IUsersAuthServices
    {
        Users? FindByUsername(string username);
        Users Create(AccountCredentialsDTO credentialsDto);
        bool RevokeToken(string username);
        Users Update(Users user);
    }
}
