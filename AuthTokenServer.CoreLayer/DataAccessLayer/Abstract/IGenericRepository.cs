using AuthTokenServer.CoreLayer.EntityLayer;
using System.Linq.Expressions;

namespace AuthTokenServer.CoreLayer.DataAccessLayer.Abstract;

public interface IGenericRepository<TEntity> where TEntity : BaseEntity
{
    IQueryable<TEntity> GetAll(bool track = true);
    Task<TEntity?> GetByIdAsync(string id, bool track = true);
    IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool track = true);
    Task CreateAsync(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);

}
