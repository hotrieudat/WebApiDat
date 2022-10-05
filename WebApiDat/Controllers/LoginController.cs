using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApiDat.Data.Model;
using WebApiDat.Data.Response;
using WebApiDat.Database.Domain;
using WebApiDat.Database.SqlServer;
using WebApiDat.Service.Token;

namespace WebApiDat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly MyDbContext Context;
        private readonly AppSettings AppSettings;
        private readonly TokenService TokenService;
        private readonly IUsersRepository UsersRepository;

        public LoginController(MyDbContext context, IUsersRepository usersRepository, IOptionsMonitor<AppSettings> optionsMonitor, TokenService tokenService)
        {
            Context = context;
            UsersRepository = usersRepository;
            AppSettings = optionsMonitor.CurrentValue;
            TokenService = tokenService;
        }

        [HttpPost("Login")]
        public IActionResult Validate(LoginModel loginModel)
        {
            var user = UsersRepository.ValidateUser(loginModel.UserName, loginModel.LoginPw);

            if (user == null)
            {
                return Ok(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid username/password",
                });
            }

            //Token
            var token = TokenService.GenerateToken(user, AppSettings.SecretKey);

            return Ok(new ApiResponse
            {
                Success = true,
                Message = "Authenticate Success",
                Data = token,
            });
        }

        [HttpPost("RenewToken")]
        public async Task<IActionResult> RenewToken(TokenModel tokenModel)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var secretKeyBytes = Encoding.UTF8.GetBytes(AppSettings.SecretKey);

            var tokenValidateParam = new TokenValidationParameters
            {
                //self-sufficient token
                ValidateIssuer = false,
                ValidateAudience = false,

                //sign token
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKeyBytes),

                ClockSkew = TimeSpan.Zero,

                ValidateLifetime = false,//ko kiem tra token het han
            };

            try
            {
                //check 1: AccessToken valid format
                var tokenInVerification = jwtTokenHandler.ValidateToken(tokenModel.AccessToken, tokenValidateParam, out var validateToken);

                //check 2: Check alg
                if (validateToken is JwtSecurityToken jwtSecurityToken)
                {
                    var result = jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha512, StringComparison.InvariantCultureIgnoreCase);

                    if (!result)
                    {
                        return Ok(new ApiResponse
                        {
                            Success = false,
                            Message = "Invalid token",
                        });
                    }
                }

                //check 3: Check access token expire?
                var utcExpireDate = long.Parse(tokenInVerification.Claims.FirstOrDefault(p => p.Type == JwtRegisteredClaimNames.Exp).Value);

                var expireDate = ConvertUnixTimeToDateTime(utcExpireDate);

                if (expireDate > DateTime.UtcNow)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has not yet expired",
                    });
                }

                //check 4: Check refresh token exsist in DB
                var storedToken = Context.RefreshTokenEntity.FirstOrDefault(x => x.Token == tokenModel.RefreshToken);
                if (storedToken == null)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token dose not exist",
                    });
                }

                //check 5: refresh token is used/revoked?
                if (storedToken.IsUsed)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has been used",
                    });
                }
                if (storedToken.IsRevoked)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Access token has been Revoked",
                    });
                }

                //check 6: Access Token id == JwtId in RefreshToken
                var jti = tokenInVerification.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
                if (storedToken.JwtId != jti)
                {
                    return Ok(new ApiResponse
                    {
                        Success = false,
                        Message = "Token doesn't match"
                    });
                }

                //Update token is used
                storedToken.IsRevoked = true;
                storedToken.IsUsed = true;
                Context.Update(storedToken);
                await Context.SaveChangesAsync();

                //create new token
                var user = await Context.UsersEntity.SingleOrDefaultAsync(u => u.UserId == storedToken.UserId);
                var token = TokenService.GenerateToken(user, AppSettings.SecretKey);

                return Ok(new ApiResponse
                {
                    Success = true,
                    Message = "Renew token success",
                    Data = token
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse
                {
                    Success = false,
                    Message = "Invalid token",
                });
            }
        }

        private DateTime ConvertUnixTimeToDateTime(long utcExpireDate)
        {
            var dateTimeInterval = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            dateTimeInterval.AddSeconds(utcExpireDate).ToUniversalTime();
            return dateTimeInterval;
        }
    }
}
