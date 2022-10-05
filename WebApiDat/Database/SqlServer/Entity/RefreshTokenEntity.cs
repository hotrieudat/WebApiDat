using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiDat.Database.SqlServer.Entity
{
    [Table("refresh_token")]
    public class RefreshTokenEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [ForeignKey(nameof(UserId))]
        public UsersEntity? UsersEntity { get; set; }

        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public string JwtId { get; set; } = null!;

        public bool IsUsed { get; set; }

        public bool IsRevoked { get; set; }

        public DateTime IssuedAt { get; set; }

        public DateTime ExpiredAt { get; set; }
    }
}
