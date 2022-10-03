
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace WebApiDat.Data.Model
{
    public class UsersModel
    {
        public string UserName { get; set; }

        public string LoginPw { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }
    }
}
