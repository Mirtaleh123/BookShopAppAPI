namespace BookShopAppAPI.Contracts;

public sealed record AdminOrderResponse(
    int Id, int UserId, DateTime OrderDate, string Status, decimal TotalPrice,
    IReadOnlyList<OrderItemResponse> Items);
