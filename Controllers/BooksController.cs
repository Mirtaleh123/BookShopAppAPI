using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBookRepository books) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll(
        [FromQuery] string? search,
        [FromQuery] string? sort,
        [FromQuery] int? categoryId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 8)
    {
        if (page < 1 || pageSize is < 1 or > 100)
            return BadRequest(new { message = "page ən azı 1, pageSize 1-100 aralığında olmalıdır." });

        var result = await books.GetPagedAsync(search, sort, categoryId, page, pageSize);
        return Ok(new
        {
            items = result.Items.Select(ToResponse),
            result.TotalCount,
            page,
            pageSize,
            totalPages = (int)Math.Ceiling(result.TotalCount / (double)pageSize)
        });
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories() => Ok(await books.GetCategoriesAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await books.GetByIdAsync(id);
        return book is null ? NotFound(new { message = "Kitab tapılmadı." }) : Ok(ToResponse(book));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateBookRequest request)
    {
        var book = new Book
        {
            Title = request.Title,
            Author = request.Author,
            Price = request.Price,
            ImageUrl = request.ImageUrl,
            About = request.About,
            Stock = request.Stock,
            CategoryId = request.CategoryId
        };
        await books.AddAsync(book);
        await books.SaveAsync();
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, ToResponse(book));
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateBookRequest request)
    {
        var book = await books.GetTrackedByIdAsync(id);
        if (book is null) return NotFound(new { message = "Kitab tapılmadı." });

        book.Title = request.Title;
        book.Author = request.Author;
        book.Price = request.Price;
        book.ImageUrl = request.ImageUrl;
        book.About = request.About;
        book.Stock = request.Stock;
        book.CategoryId = request.CategoryId;
        await books.SaveAsync();
        return Ok(ToResponse(book));
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var book = await books.GetTrackedByIdAsync(id);
        if (book is null) return NotFound(new { message = "Kitab tapılmadı." });
        book.IsDeleted = true;
        await books.SaveAsync();
        return NoContent();
    }

    [HttpPost("{id:int}/rating")]
    [Authorize]
    public async Task<IActionResult> Rate(int id, RateBookRequest request)
    {
        if (await books.GetByIdAsync(id) is null)
            return NotFound(new { message = "Kitab tapılmadı." });
        await books.RateAsync(id, CurrentUserId, request.Rating);
        await books.SaveAsync();
        return Ok(new { message = "Reytinq yadda saxlanıldı.", request.Rating });
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    private static object ToResponse(Book book) => new
    {
        book.Id,
        book.Title,
        book.Author,
        book.Price,
        book.ImageUrl,
        book.About,
        book.Stock,
        book.CategoryId,
        categoryName = book.Category?.Name,
        averageRating = book.BookRatings.Count == 0 ? 0 : book.BookRatings.Average(rating => rating.Rating)
    };
}
