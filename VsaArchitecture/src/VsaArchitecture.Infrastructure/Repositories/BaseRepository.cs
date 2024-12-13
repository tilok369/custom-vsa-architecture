using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Xml.Linq;
using VsaArchitecture.Application.Contracts.Infrastructure.Persistent;

namespace VsaArchitecture.Infrastructure.Repositories;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : class
{
    private readonly ApplicationDbContext _context;

    public BaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        await dbSet.AddAsync(entity);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> AddAsync(IEnumerable<TEntity> entityList)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        await dbSet.AddRangeAsync(entityList);
        return entityList;
    }

    public async Task<TEntity> DeleteAsync(TEntity entity)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        dbSet.Remove(entity);
        await Task.CompletedTask;
        return entity;
    }

    public async Task<TEntity> DeleteAsync(int id)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        var entity = await dbSet.FindAsync(id);
        dbSet.Remove(entity);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> DeleteAsync(IEnumerable<TEntity> entityList)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        dbSet.RemoveRange(entityList);
        await Task.CompletedTask;
        return entityList;
    }

    public async Task<int> DeleteByPropertyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await _context.Set<TEntity>()
            .Where(predicate).ExecuteDeleteAsync();
    }

    public async Task<TEntity> EditAsync(TEntity entity)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        dbSet.Update(entity);
        await Task.CompletedTask;
        return entity;
    }

    public async Task<IEnumerable<TEntity>> EditAsync(IEnumerable<TEntity> entityList)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        dbSet.UpdateRange(entityList);
        await Task.CompletedTask;
        return entityList;
    }

    public async Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        var entityList = dbSet.Where(predicate);
        await Task.CompletedTask;
        return entityList;
    }

    public async Task<TEntity> FirstAsync(Expression<Func<TEntity, bool>> predicate)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        var entity = await dbSet.FirstOrDefaultAsync(predicate);
        return entity;
    }

    public async Task<TEntity> GetAsync(int id)
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        var entity = await dbSet.FindAsync(id);
        return entity;
    }

    public async Task<IEnumerable<TEntity>> GetAsync()
    {
        DbSet<TEntity> dbSet = _context.Set<TEntity>();
        var entityList = dbSet.AsNoTracking().AsEnumerable();
        await Task.CompletedTask;
        return entityList;
    }
}
