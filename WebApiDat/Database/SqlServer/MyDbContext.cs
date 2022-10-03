using WebApiDat.Database.SqlServer.Entity;
using Microsoft.EntityFrameworkCore;

namespace WebApiDat.Database.SqlServer
{
    public class MyDbContext : DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options) { }

        public DbSet<UsersEntity> UsersEntity { get; set; }

        public DbSet<RefreshTokenEntity> RefreshTokenEntity { get; set; }
    }
}
