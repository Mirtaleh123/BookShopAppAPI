using BookShopAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public class OrdersController(IOrderService orders) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMine()
    {
        var result = await orders.GetUserOrdersAsync(CurrentUserId);
        return Ok(result.Select(order => new
        {
            order.Id,
            order.OrderDate,
            status = order.Status.ToString(),
            order.TotalPrice,
            items = order.OrderItems.Select(item => new
            {
                item.BookId,
                item.Book.Title,
                item.Quantity,
                item.Price
            })
        }));
    }

    [HttpPost("checkout")]
    public async Task<IActionResult> Checkout()
    {
        var result = await orders.CheckoutAsync(CurrentUserId);
        return result.Success
            ? Ok(new { message = "Sifariş yaradıldı." })
            : BadRequest(new { message = result.Message });
    }

    [HttpPost("{id:int}/cancel")]
    public async Task<IActionResult> Cancel(int id)
    {
        var result = await orders.CancelAsync(id, CurrentUserId);
        return result.Success
            ? Ok(new { message = "Sifariş ləğv edildi." })
            : BadRequest(new { message = result.Message });
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
