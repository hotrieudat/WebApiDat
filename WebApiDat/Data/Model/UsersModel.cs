
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiDat.Data.Model
{
    public class UsersModel
    {
        public string UserName { get; set; } = null!;

        public string LoginPw { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Name { get; set; } = null!;
    }
}
