namespace BookShopAppAPI.Contracts;

public sealed record CartResponse(IReadOnlyList<CartItemResponse> Items, decimal Total);
