using BookShopAppAPI.Repositories;

namespace BookShopAppAPI.Services;

public abstract class BaseService<TRepository>(TRepository repository) where TRepository : IRepository
{
    protected TRepository Repository { get; } = repository;

    protected Task SaveAsync() => Repository.SaveAsync();
    protected static ServiceResult Succeeded() => new(true);
    protected static ServiceResult Failed(string message) => new(false, message);
}
