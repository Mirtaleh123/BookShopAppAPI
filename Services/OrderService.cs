using BookShopAppAPI.Data;
using BookShopAppAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BookShopAppAPI.Services;

public record ServiceResult(bool Success, string? Message = null)
{
    public static ServiceResult Ok() => new(true);
    public static ServiceResult Fail(string message) => new(false, message);
}

public interface IOrderService
{
    Task<List<Order>> GetUserOrdersAsync(int userId);
    Task<ServiceResult> CheckoutAsync(int userId);
    Task<ServiceResult> CancelAsync(int orderId, int userId);
}

public class OrderService(ApplicationDbContext db) : IOrderService
{
    public Task<List<Order>> GetUserOrdersAsync(int userId) => db.Orders.AsNoTracking()
        .Include(order => order.OrderItems)
        .ThenInclude(item => item.Book)
        .Where(order => order.UserId == userId)
        .OrderByDescending(order => order.OrderDate)
        .ToListAsync();

    public async Task<ServiceResult> CheckoutAsync(int userId)
    {
        await using var transaction = await db.Database.BeginTransactionAsync();
        var items = await db.Carts.Include(cart => cart.Book)
            .Where(cart => cart.UserId == userId).ToListAsync();
        if (items.Count == 0) return ServiceResult.Fail("Səbət boşdur.");

        var invalid = items.FirstOrDefault(item => item.Book.IsDeleted || item.Quantity > item.Book.Stock);
        if (invalid is not null)
            return ServiceResult.Fail($"{invalid.Book.Title} üçün kifayət qədər stok yoxdur.");

        var order = new Order
        {
            UserId = userId,
            TotalPrice = items.Sum(item => item.Book.Price * item.Quantity),
            OrderItems = items.Select(item => new OrderItem
            {
                BookId = item.BookId,
                Quantity = item.Quantity,
                Price = item.Book.Price
            }).ToList()
        };

        foreach (var item in items) item.Book.Stock -= item.Quantity;
        db.Orders.Add(order);
        db.Carts.RemoveRange(items);
        await db.SaveChangesAsync();
        await transaction.CommitAsync();
        return ServiceResult.Ok();
    }

    public async Task<ServiceResult> CancelAsync(int orderId, int userId)
    {
        var order = await db.Orders.Include(item => item.OrderItems)
            .ThenInclude(item => item.Book)
            .FirstOrDefaultAsync(item => item.Id == orderId && item.UserId == userId);
        if (order is null) return ServiceResult.Fail("Sifariş tapılmadı.");
        if (order.Status != OrderStatus.Pending)
            return ServiceResult.Fail("Yalnız gözləyən sifarişi ləğv etmək olar.");

        foreach (var item in order.OrderItems) item.Book.Stock += item.Quantity;
        order.Status = OrderStatus.Cancelled;
        await db.SaveChangesAsync();
        return ServiceResult.Ok();
    }
}
