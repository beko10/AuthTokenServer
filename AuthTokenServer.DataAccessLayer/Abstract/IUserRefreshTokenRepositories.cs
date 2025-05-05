using AuthTokenServer.CoreLayer.DataAccessLayer.Abstract;
using AuthTokenServer.EntityLayer.Entities;

namespace AuthTokenServer.DataAccessLayer.Abstract;

public interface IUserRefreshTokenRepositories:IGenericRepository<UserRefreshToken>
{
}
