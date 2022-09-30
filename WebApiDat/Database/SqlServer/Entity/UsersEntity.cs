using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDat.Database.SqlServer.Entity
{
    [Table("users")]
    public class UsersEntity
    {
        [Key]
        [Column("user_id")]
        public string UserId { get; set; }

        [Required]
        [Column("user_name")]
        public string UserName { get; set; }

        [Required]
        [Column("login_pw")]
        public string LoginPw { get; set; }
    }
}
