using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebApiDat.Data.Model;
using WebApiDat.Data.Response;
using WebApiDat.Database.SqlServer;
using WebApiDat.Database.SqlServer.Entity;
using WebApiDat.Service.Token;
using WebApiDat.Setting;

namespace WebApiDat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyDbContext Context;
        private readonly AppSettings AppSettings;

        public LoginController(MyDbContext context, IOptionsMonitor<AppSettings> optionsMonitor)
        {
            Context = context;
            AppSettings = optionsMonitor.CurrentValue;
        }

        [HttpPost("Login")]
        public IActionResult Validate(LoginModel loginModel)
        {
            var user = Context.UsersEntity.SingleOrDefault(u => u.UserName == loginModel.UserName && u.LoginPw == loginModel.LoginPw);

            if (user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password",
                });
            }

            //Token
            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate Success",
                Data = TokenService.GenerateToken(user, AppSettings.SecretKey),
            });
        }

        //private string GenerateToken(UsersEntity usersEntity)
        //{
        //    var jwtTokenHandler = new JwtSecurityTokenHandler();
        //    var secretKeyBytes = Encoding.UTF8.GetBytes(AppSettings.SecretKey);

        //    var tokenDescription = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.Name, usersEntity.Name),
        //            new Claim(ClaimTypes.Email, usersEntity.Email),
        //            new Claim("UserName", usersEntity.UserName),
        //            new Claim("Id", usersEntity.UserId),

        //            //Roles

        //            new Claim("TokenId", Guid.NewGuid().ToString()),
        //        }),
        //        Expires = DateTime.UtcNow.AddMinutes(1),
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKeyBytes), SecurityAlgorithms.HmacSha512Signature),
        //    };

        //    var token = jwtTokenHandler.CreateToken(tokenDescription);

        //    return jwtTokenHandler.WriteToken(token);
        //}
    }
}
