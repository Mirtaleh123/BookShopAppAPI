using BookShopAppAPI.Contracts;
using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Controllers;

[ApiController]
[Authorize(Roles = "Admin")]
[Route("api/admin")]
public class AdminController(ApplicationDbContext db) : ControllerBase
{
    // MVC-dəki Admin/Index səhifəsinin JSON qarşılığıdır.
    [HttpGet("dashboard")]
    public async Task<IActionResult> Dashboard()
    {
        var orders = await db.Orders.AsNoTracking().ToListAsync();
        var totalBooks = await db.Books.CountAsync(book => !book.IsDeleted);

        return Ok(new
        {
            totalOrders = orders.Count,
            totalBooks,
            pendingOrders = orders.Count(order => order.Status == OrderStatus.Pending),
            totalRevenue = orders
                .Where(order => order.Status != OrderStatus.Cancelled)
                .Sum(order => order.TotalPrice)
        });
    }

    [HttpGet("orders")]
    public async Task<IActionResult> GetOrders()
    {
        var orders = await db.Orders.AsNoTracking()
            .Include(order => order.OrderItems)
            .ThenInclude(item => item.Book)
            .OrderByDescending(order => order.OrderDate)
            .Select(order => new
            {
                order.Id,
                order.UserId,
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
            })
            .ToListAsync();

        return Ok(orders);
    }

    [HttpPut("orders/{id:int}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, UpdateOrderStatusRequest request)
    {
        if (!Enum.TryParse<OrderStatus>(request.Status, true, out var status))
            return BadRequest(new { message = "Yanlış status. Pending, Approved, Delivered və ya Cancelled göndərin." });

        var order = await db.Orders.FindAsync(id);
        if (order is null) return NotFound(new { message = "Sifariş tapılmadı." });
        if (order.Status == OrderStatus.Cancelled)
            return Conflict(new { message = "Ləğv edilmiş sifariş dəyişdirilə bilməz." });

        order.Status = status;
        await db.SaveChangesAsync();
        return Ok(new { order.Id, status = order.Status.ToString() });
    }

    [HttpGet("ratings")]
    public async Task<IActionResult> GetRatings()
    {
        var ratings = await db.BookRatings.AsNoTracking()
            .Include(rating => rating.Book)
            .Include(rating => rating.User)
            .OrderByDescending(rating => rating.UpdatedAt)
            .Select(rating => new
            {
                rating.Id,
                rating.BookId,
                bookTitle = rating.Book.Title,
                rating.UserId,
                username = rating.User.Username,
                rating.Rating,
                rating.CreatedAt,
                rating.UpdatedAt
            })
            .ToListAsync();

        return Ok(ratings);
    }
}
