using BookShopAppAPI.Repositories.Abstract;
using BookShopAppAPI.Repositories.Base;
using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Repositories.Concrete;

public class BookRepository(ApplicationDbContext db) : BaseRepository<Book>(db), IBookRepository
{
    public async Task<(List<Book> Items, int TotalCount)> GetPagedAsync(
        string? search, string? sort, int? categoryId, int page, int pageSize)
    {
        var query = Entities.AsNoTracking()
            .Include(book => book.Category)
            .Where(book => !book.IsDeleted);

        if (!string.IsNullOrWhiteSpace(search))
            query = query.Where(book => book.Title.Contains(search) || book.Author.Contains(search));
        if (categoryId.HasValue)
            query = query.Where(book => book.CategoryId == categoryId.Value);

        query = sort switch
        {
            "price_asc" => query.OrderBy(book => book.Price),
            "price_desc" => query.OrderByDescending(book => book.Price),
            "title_desc" => query.OrderByDescending(book => book.Title),
            _ => query.OrderBy(book => book.Title)
        };

        var totalCount = await query.CountAsync();
        var items = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        return (items, totalCount);
    }

    public Task<List<Category>> GetCategoriesAsync() =>
        Db.Categories.AsNoTracking().OrderBy(category => category.Name).ToListAsync();

    public Task<Book?> GetByIdAsync(int id) => Entities.AsNoTracking()
        .Include(book => book.Category)
        .Include(book => book.BookRatings)
        .ThenInclude(rating => rating.User)
        .FirstOrDefaultAsync(book => book.Id == id && !book.IsDeleted);

    public Task<Book?> GetTrackedByIdAsync(int id) =>
        Entities.FirstOrDefaultAsync(book => book.Id == id && !book.IsDeleted);

    public async Task RateAsync(int bookId, int userId, int rating)
    {
        var existing = await Db.BookRatings
            .FirstOrDefaultAsync(item => item.BookId == bookId && item.UserId == userId);
        if (existing is null)
            await Db.BookRatings.AddAsync(new BookRating { BookId = bookId, UserId = userId, Rating = rating });
        else
        {
            existing.Rating = rating;
            existing.UpdatedAt = DateTime.UtcNow;
        }
    }

}
