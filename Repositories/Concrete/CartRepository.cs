using BookShopAppAPI.Repositories.Abstract;
using BookShopAppAPI.Repositories.Base;
using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Repositories.Concrete;

public class CartRepository(ApplicationDbContext db) : BaseRepository<Cart>(db), ICartRepository
{
    public Task<List<Cart>> GetUserItemsAsync(int userId) => Entities
        .Include(cart => cart.Book)
        .Where(cart => cart.UserId == userId && !cart.Book.IsDeleted)
        .ToListAsync();

    public Task<Book?> GetBookAsync(int bookId) =>
        Db.Books.FirstOrDefaultAsync(book => book.Id == bookId && !book.IsDeleted);

    public Task<Cart?> GetItemAsync(int userId, int bookId) =>
        Entities.FirstOrDefaultAsync(cart => cart.UserId == userId && cart.BookId == bookId);

    public Task<Cart?> GetItemByIdAsync(int id, int userId) =>
        Entities.FirstOrDefaultAsync(cart => cart.Id == id && cart.UserId == userId);
}
