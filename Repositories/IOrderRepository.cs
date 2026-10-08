using BookShopAppAPI.Models;

namespace BookShopAppAPI.Repositories;

public interface IOrderRepository : IRepository
{
    Task AddAsync(Order order);
    Task<List<Order>> GetUserOrdersAsync(int userId);
    Task<List<Cart>> GetCartItemsAsync(int userId);
    Task<Order?> GetForCancellationAsync(int orderId, int userId);
    void RemoveCartItems(IEnumerable<Cart> items);
    Task<IRepositoryTransaction> BeginTransactionAsync();
}
