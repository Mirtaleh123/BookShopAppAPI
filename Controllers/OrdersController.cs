using BookShopAppAPI.Services.Abstract;
using BookShopAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Authorize]
[Route("api/orders")]
public class OrdersController(IOrderService orders) : AuthenticatedControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetMine() => Ok(await orders.GetUserOrdersAsync(CurrentUserId));

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

}
