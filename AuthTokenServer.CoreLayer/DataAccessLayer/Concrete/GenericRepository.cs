using AuthTokenServer.CoreLayer.DataAccessLayer.Abstract;
using AuthTokenServer.CoreLayer.EntityLayer;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthTokenServer.CoreLayer.DataAccessLayer.Concrete;

public class GenericRepository<TEntity, TContext> : IGenericRepository<TEntity>
where TContext : DbContext
where TEntity : BaseEntity
{
    private readonly TContext _context;
    private DbSet<TEntity> _dbSet;
    public GenericRepository(TContext context)
    {
        _context = context;
        _dbSet = _context.Set<TEntity>();
    }

    public IQueryable<TEntity> GetAll(bool track = true)
    {
        var query = _dbSet.AsQueryable();
        if(!track)
        {
            query = query.AsNoTracking();
        }
        return query;
    }
    public IQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate, bool track = true)
    {
        var query = _dbSet.AsQueryable();
        if(!track)
        {
            query = query.AsNoTracking();
        }
        return query.Where(predicate);
    }
    public async Task<TEntity?> GetByIdAsync(string id, bool track = true)
    {
        var query = _dbSet.AsQueryable();
        if (!track)
        {
            query = query.AsNoTracking();
        }
        return await query.FirstOrDefaultAsync(x => x.Id == id);
    }
    public async Task CreateAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public void Delete(TEntity entity)
    {
        _dbSet.Remove(entity);
    }
    public void Update(TEntity entity)
    {
        _dbSet.Update(entity);  
    }

}
