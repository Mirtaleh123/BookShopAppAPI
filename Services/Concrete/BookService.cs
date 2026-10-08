using BookShopAppAPI.Services.Abstract;
using BookShopAppAPI.Services.Base;
using BookShopAppAPI.Services;
using AutoMapper;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Repositories.Abstract;

namespace BookShopAppAPI.Services.Concrete;

public sealed class BookService(IBookRepository books, IMapper mapper)
    : BaseService<IBookRepository>(books), IBookService
{
    public async Task<PagedBooksResponse> GetPagedAsync(
        string? search, string? sort, int? categoryId, int page, int pageSize)
    {
        var (items, totalCount) = await Repository.GetPagedAsync(search, sort, categoryId, page, pageSize);
        return new PagedBooksResponse(
            mapper.Map<List<BookResponse>>(items), totalCount, page, pageSize,
            (int)Math.Ceiling(totalCount / (double)pageSize));
    }

    public async Task<List<CategoryResponse>> GetCategoriesAsync()
    {
        var categories = await Repository.GetCategoriesAsync();
        return mapper.Map<List<CategoryResponse>>(categories);
    }

    public async Task<BookResponse?> GetByIdAsync(int id)
    {
        var book = await Repository.GetByIdAsync(id);
        return book is null ? null : mapper.Map<BookResponse>(book);
    }

    public async Task<BookResponse> CreateAsync(CreateBookRequest request)
    {
        var book = mapper.Map<Book>(request);
        await Repository.AddAsync(book);
        await SaveAsync();
        return mapper.Map<BookResponse>(book);
    }

    public async Task<BookResponse?> UpdateAsync(int id, UpdateBookRequest request)
    {
        var book = await Repository.GetTrackedByIdAsync(id);
        if (book is null) return null;
        mapper.Map(request, book);
        await SaveAsync();
        return mapper.Map<BookResponse>(book);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var book = await Repository.GetTrackedByIdAsync(id);
        if (book is null) return false;
        book.IsDeleted = true;
        await SaveAsync();
        return true;
    }

    public async Task<bool> RateAsync(int id, int userId, int rating)
    {
        if (await Repository.GetByIdAsync(id) is null) return false;
        await Repository.RateAsync(id, userId, rating);
        await SaveAsync();
        return true;
    }
}
