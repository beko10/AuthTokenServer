using System.Linq.Expressions;

namespace AuthTokenServer.CoreLayer.DataAccessLayer.Abstract;

public interface IGenericRepository<TEntity> where TEntity : class
{
    IQueryable<TEntity> GetAll(bool track = true);
    Task<TEntity?> GetByIdAsync(string id, bool track = true);
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool track = true);
    Task Create(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);

}
