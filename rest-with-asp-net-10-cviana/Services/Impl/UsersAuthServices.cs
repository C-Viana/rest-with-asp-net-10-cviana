using Microsoft.AspNetCore.Identity;
using rest_with_asp_net_10_cviana.Auth.Contract;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;
using rest_with_asp_net_10_cviana.Repositories.UsersRepository;

namespace rest_with_asp_net_10_cviana.Services.Impl
{
    public class UsersAuthServices(IUsersRepository usersRepository, IPasswordHasher passwordHasher) : IUsersAuthServices
    {
        private readonly IUsersRepository _usersRepository = usersRepository;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;

        public Users Create(AccountCredentialsDTO credentialsDto)
        {
            if (credentialsDto == null) throw new ArgumentNullException(nameof(credentialsDto), "Required data is missing");
            Users entity = new()
            {
                Username = credentialsDto.Username,
                FullName = credentialsDto.Fullname,
                Password = _passwordHasher.Hash(credentialsDto.Password),
                RefreshToken = string.Empty,
                RefreshTokenExpiryTime = null
            };
            return _usersRepository.Create(entity);
        }

        public Users FindByUsername(string username)
        {
            Users entity = _usersRepository.FindByUsername(username);
            return entity;
        }

        public bool RevokeToken(string username)
        {
            Users entity = _usersRepository.FindByUsername(username);
            if (entity == null) return false;
            entity.RefreshToken = null;
            entity.RefreshTokenExpiryTime = null;
            _usersRepository.Update(entity);
            return true;
        }

        public Users Update(Users user)
        {
            return _usersRepository.Update(user);
        }
    }
}
