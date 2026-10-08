using BookShopAppAPI.Models;

namespace BookShopAppAPI.Repositories;

public interface ICartRepository : IRepository
{
    Task AddAsync(Cart cart);
    void Remove(Cart cart);
    Task<List<Cart>> GetUserItemsAsync(int userId);
    Task<Book?> GetBookAsync(int bookId);
    Task<Cart?> GetItemAsync(int userId, int bookId);
    Task<Cart?> GetItemByIdAsync(int id, int userId);
}
