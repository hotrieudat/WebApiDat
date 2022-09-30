using WebApiDat.Database.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiDat.Data.Model;

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
                var data = UsersRepository.GetUserById(name);

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

        [HttpPut("{id}")]
        public IActionResult UpdateUser(string id, UsersModel usersModel)
        {
            try
            {
                UsersRepository.UpdateUser(id, usersModel);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete]
        public IActionResult DeleteUser(string id)
        {
            try
            {
                UsersRepository.DeleteUser(id);
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
