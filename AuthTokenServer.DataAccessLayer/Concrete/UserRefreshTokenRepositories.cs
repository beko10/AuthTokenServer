using AuthTokenServer.CoreLayer.DataAccessLayer.Concrete;
using AuthTokenServer.DataAccessLayer.Abstract;
using AuthTokenServer.DataAccessLayer.Context;
using AuthTokenServer.EntityLayer.Entities;

namespace AuthTokenServer.DataAccessLayer.Concrete;

public class UserRefreshTokenRepositories : GenericRepository<UserRefreshToken, AppDbContext>, IUserRefreshTokenRepositories
{
    public UserRefreshTokenRepositories(AppDbContext context) : base(context)
    {
    }
}
