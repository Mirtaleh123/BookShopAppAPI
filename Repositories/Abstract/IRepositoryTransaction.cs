namespace BookShopAppAPI.Repositories.Abstract;

public interface IRepositoryTransaction : IAsyncDisposable
{
    Task CommitAsync();
}
