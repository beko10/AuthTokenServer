namespace AuthTokenServer.CoreLayer.DataAccessLayer.UnitOfWork;

public interface IUnitOfWork : IAsyncDisposable
{
    Task<int> SaveChangesAsync();

}
