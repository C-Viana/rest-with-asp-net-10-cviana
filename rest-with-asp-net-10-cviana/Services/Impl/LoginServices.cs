using Mapster;
using rest_with_asp_net_10_cviana.Auth.Config;
using rest_with_asp_net_10_cviana.Auth.Contract;
using rest_with_asp_net_10_cviana.Data.DTO.V1;
using rest_with_asp_net_10_cviana.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace rest_with_asp_net_10_cviana.Services.Impl
{
    public class LoginServices : ILoginServices
    {
        private const string DATE_FORMAT = "yyy-MM-dd HH:mm:ss";
        private readonly IUsersAuthServices _userAuthService;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenGenerator _tokenGenerator;
        private readonly TokenConfiguration _tokenConfiguration;
        private readonly ILogger<LoginServices> _logger;

        public LoginServices(IUsersAuthServices userAuthService, IPasswordHasher passwordHasher, ITokenGenerator tokenGenerator, TokenConfiguration tokenConfiguration, ILogger<LoginServices> logger)
        {
            _userAuthService = userAuthService;
            _passwordHasher = passwordHasher;
            _tokenGenerator = tokenGenerator;
            _tokenConfiguration = tokenConfiguration;
            _logger = logger;
        }

        public AccountCredentialsDTO Create(AccountCredentialsDTO user)
        {
            Users createdUser = _userAuthService.Create(user);
            user.Password = "******";
            return user;
        }

        public bool RevokeToken(string username)
        {
            return _userAuthService.RevokeToken(username);
        }

        public TokenDTO ValidateCredentials(UsersDTO userDto)
        {
            var user = _userAuthService.FindByUsername(userDto.Username);
            if (user == null) return null;
            if(!_passwordHasher.Verify(userDto.Password, user.Password)) return null;
            return GenerateToken(user);
        }

        public TokenDTO ValidateCredentials(TokenDTO token)
        {
            var principal = _tokenGenerator.GetPrincipalFromExpiredToken(token.AccessToken);
            var username = principal.Identity?.Name;
            var user = _userAuthService.FindByUsername(username);
            if(user == null || user.RefreshToken != token.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                return null;
            return GenerateToken(user, principal.Claims);
        }

        private TokenDTO GenerateToken(Users user, IEnumerable<Claim>? existingClaims = null)
        {
            var claims = existingClaims?.ToList() ?? [
                //new Claim(ClaimTypes.Name, user.Username),
                //new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")), //The parameter "N" converts a GUID to a clean text
                new Claim(JwtRegisteredClaimNames.UniqueName, user.Username)
            ];
            var accessToken = _tokenGenerator.GenerateAccessToken(claims);
            var refreshToken = _tokenGenerator.GenerateRefreshToken();
            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(_tokenConfiguration.DaysToExpiry); ;
            _userAuthService.Update(user);

            var currentDate = DateTime.UtcNow;
            var expirationAccessTime = currentDate.AddMinutes(_tokenConfiguration.Minutes);

            return new TokenDTO { 
                Authenticated = true,
                Created = currentDate.ToString(DATE_FORMAT),
                Expiration = expirationAccessTime.ToString(DATE_FORMAT),
                AccessToken = accessToken,
                RefreshToken = refreshToken
            };
        }
    }
}
