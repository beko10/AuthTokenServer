namespace AuthTokenServer.CoreLayer.DataAccessLayer.Abstract;

public interface IUnitOfWork:IAsyncDisposable
{
    Task<int> CommitAsync();

}
