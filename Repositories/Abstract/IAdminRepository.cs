using BookShopAppAPI.Repositories.Base;
using BookShopAppAPI.Repositories;
using BookShopAppAPI.Models;

namespace BookShopAppAPI.Repositories.Abstract;

public interface IAdminRepository : IRepository
{
    Task<Order?> FindByIdAsync(int id);
    Task<List<OrderFinancialSummary>> GetOrderSummariesAsync();
    Task<int> GetActiveBookCountAsync();
    Task<List<Order>> GetOrdersAsync();
    Task<List<BookRating>> GetRatingsAsync();
}
