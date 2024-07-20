using CheckDrive.Domain.Entities;
using CheckDrive.Domain.Interfaces.Auth;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CheckDrive.Infrastructure.JwtToken
{
    public class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
    {
        private readonly JwtOptions _options = options.Value;

        public string GenerateToken(Account account)
        {
            var claimForToken = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, account.Id.ToString()),
                new Claim(ClaimTypes.Role, account.RoleId.ToString())
            };

            switch (account.RoleId)
            {
                case 1:
                    claimForToken.Add(new Claim("Admin", "true"));
                    break;
                case 2:
                    claimForToken.Add(new Claim("Driver", "true"));
                    break;
                case 3:
                    claimForToken.Add(new Claim("Doctor", "true"));
                    break;
                case 4:
                    claimForToken.Add(new Claim("Operator", "true"));
                    break;
                case 5:
                    claimForToken.Add(new Claim("Dispatcher", "true"));
                    break;
                case 6:
                    claimForToken.Add(new Claim("Mechanic", "true"));
                    break;
            }

            var securityKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.SecretKey));
            var signingCredentials = new SigningCredentials(securityKey,
                SecurityAlgorithms.HmacSha256);

            var claimsForToken = new List<Claim>();
            claimsForToken.Add(new Claim("sub", account.FirstName));
            claimsForToken.Add(new Claim("name", account.LastName));

            var jwtSecurityToken = new JwtSecurityToken(
                claims: claimForToken,
                signingCredentials: signingCredentials,
                expires: DateTime.UtcNow.AddHours(_options.ExpiresHours));

            var token = new JwtSecurityTokenHandler()
                .WriteToken(jwtSecurityToken);

            return token;
        }
    }
}
