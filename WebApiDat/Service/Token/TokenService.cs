using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using WebApiDat.Database.SqlServer.Entity;
using WebApiDat.Data.Model;
using System.Security.Cryptography;
using WebApiDat.Database.Domain;
using WebApiDat.Database.SqlServer.Repository;

namespace WebApiDat.Service.Token
{
    public class TokenService
    {
        private readonly ILogger<RefreshTokenRepository> Logger;

        private IRefreshTokenRepository RefreshTokenRepository;

        public TokenService(ILogger<RefreshTokenRepository> logger, IRefreshTokenRepository refreshTokenRepository) 
        {
            Logger = logger;
            RefreshTokenRepository = refreshTokenRepository;
        }

        public TokenModel GenerateToken(UsersEntity usersEntity, string secretKey)
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
                Expires = DateTime.UtcNow.AddSeconds(120),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            var accessToken = jwtTokenHandler.WriteToken(token);

            var refreshToken = GenerateRefeshToken();

            //save database
            RefreshTokenRepository.Save(
                Guid.NewGuid(),
                usersEntity.UserId,
                refreshToken,
                token.Id,
                false,
                false,
                DateTime.UtcNow,
                DateTime.UtcNow.AddMinutes(3));

            Logger.LogInformation("Generate Token Success!!!");
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
