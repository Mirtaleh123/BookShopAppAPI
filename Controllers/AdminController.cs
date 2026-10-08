using BookShopAppAPI.Services.Abstract;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin")]
public class AdminController(IAdminService admin) : ControllerBase
{
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard() => Ok(await admin.GetDashboardAsync());

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders() => Ok(await admin.GetOrdersAsync());

    [HttpGet("ratings")]
    public async Task<IActionResult> GetRatings() => Ok(await admin.GetRatingsAsync());

    [HttpPut("orders/{id:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusRequest request)
    {
        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
            return BadRequest(new { message = "Yanlış status. Pending, Approved, Delivered və ya Cancelled göndərin." });

        return await admin.UpdateOrderStatusAsync(id, status) switch
        {
            UpdateStatusOutcome.NotFound => NotFound(new { message = "Sifariş tapılmadı." }),
            UpdateStatusOutcome.Cancelled => Conflict(new { message = "Ləğv edilmiş sifariş dəyişdirilə bilməz." }),
            _ => Ok(new { Id = id, status = status.ToString() })
        };
    }
}
