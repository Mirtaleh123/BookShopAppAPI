using BookShopAppAPI.Repositories.Abstract;
using BookShopAppAPI.Repositories.Base;
using BookShopAppAPI.Repositories;
using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Repositories.Concrete;

public sealed class AdminRepository(ApplicationDbContext db) : BaseRepository<Order>(db), IAdminRepository
{
    public Task<List<OrderFinancialSummary>> GetOrderSummariesAsync() => Entities.AsNoTracking()
        .Select(order => new OrderFinancialSummary(order.Status, order.TotalPrice))
        .ToListAsync();

    public Task<int> GetActiveBookCountAsync() =>
        Db.Books.CountAsync(book => !book.IsDeleted);

    public Task<List<Order>> GetOrdersAsync() => Entities.AsNoTracking()
        .Include(order => order.OrderItems)
        .ThenInclude(item => item.Book)
        .OrderByDescending(order => order.OrderDate)
        .ToListAsync();

    public Task<List<BookRating>> GetRatingsAsync() => Db.BookRatings.AsNoTracking()
        .Include(rating => rating.Book)
        .Include(rating => rating.User)
        .OrderByDescending(rating => rating.UpdatedAt)
        .ToListAsync();
}
