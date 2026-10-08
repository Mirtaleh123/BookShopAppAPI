using BookShopAppAPI.Services.Abstract;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/cart")]
public class CartController(ICartService cart) : AuthenticatedControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMine() => Ok(await cart.GetUserCartAsync(CurrentUserId));

    [HttpPost("items")]
    public async Task<IActionResult> Add(AddCartItemRequest request)
    {
        var result = await cart.AddAsync(CurrentUserId, request);
        return result.Outcome switch
        {
            CartAddOutcome.BookNotFound => NotFound(new { message = "Kitab tapılmadı." }),
            CartAddOutcome.InsufficientStock => Conflict(new { message = $"Stokda yalnız {result.Stock} ədəd var." }),
            _ => Ok(new { message = "Kitab səbətə əlavə edildi.", quantity = result.Quantity })
        };
    }

    [HttpDelete("items/{id:int}")]
    public async Task<IActionResult> Remove(int id) => await cart.RemoveAsync(CurrentUserId, id)
        ? NoContent()
        : NotFound(new { message = "Səbət elementi tapılmadı." });
}
