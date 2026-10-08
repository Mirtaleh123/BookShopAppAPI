using BookShopAppAPI.Repositories.Base;
using BookShopAppAPI.Models;

namespace BookShopAppAPI.Repositories.Abstract;

public interface IBookRepository : IRepository
{
    Task AddAsync(Book book);
    Task<(List<Book> Items, int TotalCount)> GetPagedAsync(string? search, string? sort, int? categoryId, int page, int pageSize);
    Task<List<Category>> GetCategoriesAsync();
    Task<Book?> GetByIdAsync(int id);
    Task<Book?> GetTrackedByIdAsync(int id);
    Task RateAsync(int bookId, int userId, int rating);
}
