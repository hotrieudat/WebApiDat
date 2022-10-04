using System;
using WebApiDat.Database.SqlServer.Entity;

namespace WebApiDat.Database.Domain
{
    public interface IRefreshTokenRepository
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
