namespace BookShopAppAPI.Repositories;

public interface IRepositoryTransaction : IAsyncDisposable
{
    Task CommitAsync();
}
