using BookShopAppAPI.Services;
using BookShopAppAPI.Contracts;

namespace BookShopAppAPI.Services.Abstract;

public interface ICartService
{
    Task<CartResponse> GetUserCartAsync(int userId);
    Task<CartAddResult> AddAsync(int userId, AddCartItemRequest request);
    Task<bool> RemoveAsync(int userId, int itemId);
}
