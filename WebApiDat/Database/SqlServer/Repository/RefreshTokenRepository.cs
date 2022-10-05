using System;
using WebApiDat.Database.Domain;
using WebApiDat.Database.SqlServer.Entity;

namespace WebApiDat.Database.SqlServer.Repository
{
    public class RefreshTokenRepository : Repository<RefreshTokenEntity>, IRefreshTokenRepository
    {
        private readonly MyDbContext myContext;
        public RefreshTokenRepository(MyDbContext context) : base(context)
        {
            myContext = context;
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

            myContext.RefreshTokenEntity.Add(refreshTokenEntity);
            myContext.SaveChanges();
        }
    }
}
