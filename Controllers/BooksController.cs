using BookShopAppAPI.Services.Abstract;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Route("api/books")]
public class BooksController(IBookService books) : AuthenticatedControllerBase
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

        return Ok(await books.GetPagedAsync(search, sort, categoryId, page, pageSize));
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories() => Ok(await books.GetCategoriesAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var book = await books.GetByIdAsync(id);
        return book is null ? NotFound(new { message = "Kitab tapılmadı." }) : Ok(book);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateBookRequest request)
    {
        var book = await books.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
    }

    [HttpPut("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateBookRequest request)
    {
        var book = await books.UpdateAsync(id, request);
        return book is null ? NotFound(new { message = "Kitab tapılmadı." }) : Ok(book);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        return await books.DeleteAsync(id)
            ? NoContent()
            : NotFound(new { message = "Kitab tapılmadı." });
    }

    [HttpPost("{id:int}/rating")]
    [Authorize]
    public async Task<IActionResult> Rate(int id, RateBookRequest request)
    {
        return await books.RateAsync(id, CurrentUserId, request.Rating)
            ? Ok(new { message = "Reytinq yadda saxlanıldı.", request.Rating })
            : NotFound(new { message = "Kitab tapılmadı." });
    }
}
