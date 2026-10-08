using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;

namespace BookShopAppAPI.Services;

public interface IAdminService
{
    Task<DashboardResponse> GetDashboardAsync();
    Task<List<AdminOrderResponse>> GetOrdersAsync();
    Task<List<RatingResponse>> GetRatingsAsync();
    Task<UpdateStatusOutcome> UpdateOrderStatusAsync(int orderId, OrderStatus status);
}
