using System;
using WebApiDat.Database.SqlServer.Entity;
using WebApiDat.Database.SqlServer.Repository;

namespace WebApiDat.Database.Domain
{
    public interface IRefreshTokenRepository : IRepository<RefreshTokenEntity>
    {
        void Save(
            Guid Id,
            string UserId,
            string Token,
            string JwtId,
            bool IsUsed,
            bool IsRevoked,
            DateTime IssuedAt,
            DateTime ExpiredAt
            );
    }
}
