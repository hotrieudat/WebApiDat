using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;
using WebApiDat.Database.SqlServer.Entity;
using WebApiDat.Setting;

namespace WebApiDat.Service.Token
{
    public class TokenService
    {
        static public string GenerateToken(UsersEntity usersEntity, string secretKey)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(secretKey);

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, usersEntity.Name),
                    new Claim(ClaimTypes.Email, usersEntity.Email),
                    new Claim("UserName", usersEntity.UserName),
                    new Claim("Id", usersEntity.UserId),

                    //Roles

                    new Claim("TokenId", Guid.NewGuid().ToString()),
                }),
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature),
            };

            var token = jwtTokenHandler.CreateToken(tokenDescription);

            return jwtTokenHandler.WriteToken(token);
        }
    }
}
