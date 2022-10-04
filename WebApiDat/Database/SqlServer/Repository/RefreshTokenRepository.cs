using System;
using WebApiDat.Database.Domain;
using WebApiDat.Database.SqlServer.Entity;

namespace WebApiDat.Database.SqlServer.Repository
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly MyDbContext Context;
        public RefreshTokenRepository(MyDbContext context)
        {
            Context = context;
        }

        public void Save(Guid id, string userId, string token, string jwtId, bool isUsed, bool isRevoked, DateTime issuedAt, DateTime expiredAt)
        {
            var refreshTokenEntity = new RefreshTokenEntity
            {
                Id = id,
                UserId = userId,
                Token = token,
                JwtId = jwtId,
                IsUsed = isUsed,
                IsRevoked = isRevoked,
                IssuedAt = issuedAt,
                ExpiredAt = expiredAt,
            };

            Context.RefreshTokenEntity.Add(refreshTokenEntity);
            Context.SaveChanges();
        }
    }
}
