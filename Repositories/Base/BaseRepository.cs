using BookShopAppAPI.Data;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Repositories;

public abstract class BaseRepository<TEntity>(ApplicationDbContext db) : IBaseRepository<TEntity>
    where TEntity : class
{
    protected ApplicationDbContext Db { get; } = db;
    protected DbSet<TEntity> Entities => Db.Set<TEntity>();

    public Task<TEntity?> FindByIdAsync(int id) => Entities.FindAsync(id).AsTask();
    public async Task AddAsync(TEntity entity) => await Entities.AddAsync(entity);
    public void Remove(TEntity entity) => Entities.Remove(entity);
    public async Task SaveAsync() => await Db.SaveChangesAsync();
}
