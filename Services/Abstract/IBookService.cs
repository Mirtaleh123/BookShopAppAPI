using BookShopAppAPI.Services;
using BookShopAppAPI.Contracts;

namespace BookShopAppAPI.Services.Abstract;

public interface IBookService
{
    Task<PagedBooksResponse> GetPagedAsync(string? search, string? sort, int? categoryId, int page, int pageSize);
    Task<List<CategoryResponse>> GetCategoriesAsync();
    Task<BookResponse?> GetByIdAsync(int id);
    Task<BookResponse> CreateAsync(CreateBookRequest request);
    Task<BookResponse?> UpdateAsync(int id, UpdateBookRequest request);
    Task<bool> DeleteAsync(int id);
    Task<bool> RateAsync(int id, int userId, int rating);
}
