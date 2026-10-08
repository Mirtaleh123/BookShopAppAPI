namespace BookShopAppAPI.Contracts;

public sealed record OrderResponse(
    int Id, DateTime OrderDate, string Status, decimal TotalPrice,
    IReadOnlyList<OrderItemResponse> Items);
