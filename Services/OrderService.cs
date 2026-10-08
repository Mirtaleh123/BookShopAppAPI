using AutoMapper;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Repositories;

namespace BookShopAppAPI.Services;

public sealed class OrderService(IOrderRepository orders, IMapper mapper)
    : BaseService<IOrderRepository>(orders), IOrderService
{
    public async Task<List<OrderResponse>> GetUserOrdersAsync(int userId)
    {
        var orders = await Repository.GetUserOrdersAsync(userId);
        return mapper.Map<List<OrderResponse>>(orders);
    }

    public async Task<ServiceResult> CheckoutAsync(int userId)
    {
        await using var transaction = await Repository.BeginTransactionAsync();
        var items = await Repository.GetCartItemsAsync(userId);
        if (items.Count == 0) return Failed(ServiceMessages.EmptyCart);

        var invalid = items.FirstOrDefault(item => item.Book.IsDeleted || item.Quantity > item.Book.Stock);
        if (invalid is not null)
            return Failed(ServiceMessages.InsufficientStock(invalid.Book.Title));

        var totalPrice = items.Sum(item => item.Book.Price * item.Quantity);
        var order = new Order
        {
            UserId = userId,
            TotalPrice = totalPrice,
            OrderItems = mapper.Map<List<OrderItem>>(items)
        };

        foreach (var item in items) item.Book.Stock -= item.Quantity;
        await Repository.AddAsync(order);
        Repository.RemoveCartItems(items);
        await SaveAsync();
        await transaction.CommitAsync();
        return Succeeded();
    }

    public async Task<ServiceResult> CancelAsync(int orderId, int userId)
    {
        var order = await Repository.GetForCancellationAsync(orderId, userId);
        if (order is null) return Failed(ServiceMessages.OrderNotFound);
        if (order.Status != OrderStatus.Pending)
            return Failed(ServiceMessages.OrderNotPending);

        foreach (var item in order.OrderItems) item.Book.Stock += item.Quantity;
        order.Status = OrderStatus.Cancelled;
        await SaveAsync();
        return Succeeded();
    }
}
