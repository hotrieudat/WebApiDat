using WebApiDat.Database.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDat.Data.Model;
using Microsoft.AspNetCore.Authorization;

namespace WebApiDat.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository UsersRepository;
        public UsersController(IUsersRepository usersRepository)
        {
            UsersRepository = usersRepository;
        }

        [HttpGet]
        [Authorize]
        public IActionResult GetAllUser()
        {
            try
            {
                return Ok(UsersRepository.GetAllUser());
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetUserById(string name)
        {
            try
            {
                var data = UsersRepository.GetUserByUsername(name);

                if (data != null)
                {
                    return Ok(data);
                }
                return NotFound();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut("{userName}")]
        public IActionResult UpdateUser(string userName, UsersModel usersModel)
        {
            try
            {
                UsersRepository.UpdateUser(userName, usersModel);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{userName}")]
        public IActionResult DeleteUser(string userName)
        {
            try
            {
                UsersRepository.DeleteUser(userName);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        public IActionResult CreateUser(UsersModel usersModel)
        {
            try
            {
                var data = UsersRepository.AddUser(usersModel);
                return Ok(data);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
