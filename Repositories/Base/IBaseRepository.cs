namespace BookShopAppAPI.Repositories.Base;

public interface IBaseRepository<TEntity> : IRepository where TEntity : class
{
    Task<TEntity?> FindByIdAsync(int id);
    Task AddAsync(TEntity entity);
    void Remove(TEntity entity);
}
