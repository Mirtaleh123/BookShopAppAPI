namespace BookShopAppAPI.Contracts;

public sealed record CartItemResponse(
    int Id, int BookId, string Title, decimal Price, int Quantity, int Stock);
