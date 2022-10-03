using WebApiDat.Database.SqlServer.Entity;

namespace WebApiDat.Database.Domain
{
    public interface IRefreshTokenRepository
    {
        void CreateRefreshToken(RefreshTokenEntity refreshTokenEntity);
    }
}
