using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Repositories;

public sealed class OrderRepository(ApplicationDbContext db) : BaseRepository<Order>(db), IOrderRepository
{
    public Task<List<Order>> GetUserOrdersAsync(int userId) => Entities.AsNoTracking()
        .Include(order => order.OrderItems)
        .ThenInclude(item => item.Book)
        .Where(order => order.UserId == userId)
        .OrderByDescending(order => order.OrderDate)
        .ToListAsync();

    public Task<List<Cart>> GetCartItemsAsync(int userId) => Db.Carts
        .Include(cart => cart.Book)
        .Where(cart => cart.UserId == userId)
        .ToListAsync();

    public Task<Order?> GetForCancellationAsync(int orderId, int userId) => Entities
        .Include(order => order.OrderItems)
        .ThenInclude(item => item.Book)
        .FirstOrDefaultAsync(order => order.Id == orderId && order.UserId == userId);

    public void RemoveCartItems(IEnumerable<Cart> items) => Db.Carts.RemoveRange(items);

    public async Task<IRepositoryTransaction> BeginTransactionAsync() =>
        new RepositoryTransaction(await Db.Database.BeginTransactionAsync());

}
