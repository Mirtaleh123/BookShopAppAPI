using BookShopAppAPI.Services.Abstract;
using BookShopAppAPI.Services.Base;
using BookShopAppAPI.Services;
using AutoMapper;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Repositories.Abstract;

namespace BookShopAppAPI.Services.Concrete;

public sealed class AdminService(IAdminRepository admin, IMapper mapper)
    : BaseService<IAdminRepository>(admin), IAdminService
{
    public async Task<DashboardResponse> GetDashboardAsync()
    {
        var orders = await Repository.GetOrderSummariesAsync();
        var totalBooks = await Repository.GetActiveBookCountAsync();

        return new DashboardResponse(
            orders.Count,
            totalBooks,
            orders.Count(order => order.Status == OrderStatus.Pending),
            orders.Where(order => order.Status != OrderStatus.Cancelled).Sum(order => order.TotalPrice));
    }

    public async Task<List<AdminOrderResponse>> GetOrdersAsync()
    {
        var orders = await Repository.GetOrdersAsync();
        return mapper.Map<List<AdminOrderResponse>>(orders);
    }

    public async Task<List<RatingResponse>> GetRatingsAsync()
    {
        var ratings = await Repository.GetRatingsAsync();
        return mapper.Map<List<RatingResponse>>(ratings);
    }

    public async Task<UpdateStatusOutcome> UpdateOrderStatusAsync(int orderId, OrderStatus status)
    {
        var order = await Repository.FindByIdAsync(orderId);
        if (order is null) return UpdateStatusOutcome.NotFound;
        if (order.Status == OrderStatus.Cancelled) return UpdateStatusOutcome.Cancelled;

        order.Status = status;
        await SaveAsync();
        return UpdateStatusOutcome.Updated;
    }
}
