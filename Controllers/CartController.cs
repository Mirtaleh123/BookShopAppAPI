using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/cart")]
public class CartController(ICartRepository cart) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var items = await cart.GetUserItemsAsync(CurrentUserId);
        return Ok(new
        {
            items = items.Select(item => new
            {
                item.Id,
                item.BookId,
                item.Book.Title,
                item.Book.Price,
                item.Quantity,
                item.Book.Stock
            }),
            total = items.Sum(item => item.Book.Price * item.Quantity)
        });
    }

    [HttpPost("items")]
    public async Task<IActionResult> Add(AddCartItemRequest request)
    {
        var book = await cart.GetBookAsync(request.BookId);
        if (book is null) return NotFound(new { message = "Kitab tapılmadı." });

        var existing = await cart.GetItemAsync(CurrentUserId, request.BookId);
        var newQuantity = (existing?.Quantity ?? 0) + request.Quantity;
        if (newQuantity > book.Stock)
            return Conflict(new { message = $"Stokda yalnız {book.Stock} ədəd var." });

        if (existing is null)
            cart.Add(new Cart { UserId = CurrentUserId, BookId = request.BookId, Quantity = request.Quantity });
        else
            existing.Quantity = newQuantity;

        await cart.SaveAsync();
        return Ok(new { message = "Kitab səbətə əlavə edildi.", quantity = newQuantity });
    }

    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> Remove(int id)
    {
        var item = await cart.GetItemByIdAsync(id, CurrentUserId);
        if (item is null) return NotFound(new { message = "Səbət elementi tapılmadı." });
        cart.Remove(item);
        await cart.SaveAsync();
        return NoContent();
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
