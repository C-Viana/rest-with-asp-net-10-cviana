using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;

namespace rest_with_asp_net_10_cviana.Services
{
    public interface ILoginServices
    {
        TokenDTO? ValidateCredentials(UsersDTO user);
        TokenDTO? ValidateCredentials(TokenDTO token);
        bool RevokeToken(string username);
        AccountCredentialsDTO Create(AccountCredentialsDTO user);
    }
}
