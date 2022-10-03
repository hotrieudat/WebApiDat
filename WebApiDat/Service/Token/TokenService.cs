using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using WebApiDat.Database.SqlServer.Entity;
using WebApiDat.Setting;
using WebApiDat.Data.Model;
using System.Security.Cryptography;
using WebApiDat.Database.SqlServer;
using System.Threading.Tasks;

namespace WebApiDat.Service.Token
{
    public class TokenService
    {
        private readonly MyDbContext Context;
        public TokenService(MyDbContext context) 
        {
            Context = context;
        }
        public async Task<TokenModel> GenerateToken(UsersEntity usersEntity, string secretKey)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usersEntity.Name),
                    new Claim(JwtRegisteredClaimNames.Email, usersEntity.Email),
                    new Claim(JwtRegisteredClaimNames.Sub, usersEntity.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim("UserName", usersEntity.UserName),
                    new Claim("Id", usersEntity.UserId),

                    //Roles

                    //new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddSeconds(20),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = GenerateRefeshToken();

            //save database
            var refreshTokenEntity = new RefreshTokenEntity
            {
                Id = Guid.NewGuid(),
                JwtId = token.Id,
                UserId = usersEntity.UserId,
                Token = refreshToken,
                IsUsed = false,
                IsRevoked = false,
                IssuedAt = DateTime.UtcNow,
                ExpiredAt = DateTime.UtcNow.AddMinutes(3),
            };

            await Context.RefreshTokenEntity.AddAsync(refreshTokenEntity);
            await Context.SaveChangesAsync();

            return new TokenModel
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
            };
        }

        private string GenerateRefeshToken()
        {
            var random = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(random);

                return Convert.ToBase64String(random);
            }
        }
    }
}
