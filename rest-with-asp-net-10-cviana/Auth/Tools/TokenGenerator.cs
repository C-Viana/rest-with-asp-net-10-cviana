using Microsoft.IdentityModel.Tokens;
using rest_with_asp_net_10_cviana.Auth.Config;
using rest_with_asp_net_10_cviana.Auth.Contract;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace rest_with_asp_net_10_cviana.Auth.Tools
{
    public class TokenGenerator(TokenConfiguration tokenConfiguration) : ITokenGenerator
    {
        private readonly TokenConfiguration _tokenConfiguration = tokenConfiguration;

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            SymmetricSecurityKey secretKey = new(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret));
            SigningCredentials signinCredentials = new(secretKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken tokenOptions = new(
                issuer: _tokenConfiguration.Issuer,
                audience: _tokenConfiguration.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(_tokenConfiguration.Minutes),
                signingCredentials: signinCredentials
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenOptions);
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var randomNumberGenerator = RandomNumberGenerator.Create())
            {
                randomNumberGenerator.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            TokenValidationParameters parameters = new()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenConfiguration.Secret)),
                ValidateLifetime = false
            };

            JwtSecurityTokenHandler handler = new();
            ClaimsPrincipal principal = handler.ValidateToken(token, parameters, out SecurityToken securityToken);

            if (securityToken is not JwtSecurityToken jwtSecurityToken
                || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
            )
                throw new SecurityTokenException("Invalid token");

            return principal;
        }
    }
}
