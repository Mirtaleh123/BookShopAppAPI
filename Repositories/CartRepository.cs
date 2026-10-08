using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Repositories;

public interface ICartRepository
{
    Task<List<Cart>> GetUserItemsAsync(int userId);
    Task<Book?> GetBookAsync(int bookId);
    Task<Cart?> GetItemAsync(int userId, int bookId);
    Task<Cart?> GetItemByIdAsync(int id, int userId);
    void Add(Cart cart);
    void Remove(Cart cart);
    Task SaveAsync();
}

public class CartRepository(ApplicationDbContext db) : ICartRepository
{
    public Task<List<Cart>> GetUserItemsAsync(int userId) => db.Carts
        .Include(cart => cart.Book)
        .Where(cart => cart.UserId == userId && !cart.Book.IsDeleted)
        .ToListAsync();

    public Task<Book?> GetBookAsync(int bookId) =>
        db.Books.FirstOrDefaultAsync(book => book.Id == bookId && !book.IsDeleted);

    public Task<Cart?> GetItemAsync(int userId, int bookId) =>
        db.Carts.FirstOrDefaultAsync(cart => cart.UserId == userId && cart.BookId == bookId);

    public Task<Cart?> GetItemByIdAsync(int id, int userId) =>
        db.Carts.FirstOrDefaultAsync(cart => cart.Id == id && cart.UserId == userId);

    public void Add(Cart cart) => db.Carts.Add(cart);
    public void Remove(Cart cart) => db.Carts.Remove(cart);
    public async Task SaveAsync() => await db.SaveChangesAsync();
}
