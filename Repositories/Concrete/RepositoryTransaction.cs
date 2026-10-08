using BookShopAppAPI.Repositories.Abstract;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookShopAppAPI.Repositories.Concrete;

public sealed class RepositoryTransaction(IDbContextTransaction transaction) : IRepositoryTransaction
{
    public Task CommitAsync() => transaction.CommitAsync();
    public ValueTask DisposeAsync() => transaction.DisposeAsync();
}
