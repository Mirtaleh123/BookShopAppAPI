using BookShopAppAPI.Contracts;

namespace BookShopAppAPI.Services;

public interface IOrderService
{
    Task<List<OrderResponse>> GetUserOrdersAsync(int userId);
    Task<ServiceResult> CheckoutAsync(int userId);
    Task<ServiceResult> CancelAsync(int orderId, int userId);
}
