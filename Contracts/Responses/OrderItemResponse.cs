namespace BookShopAppAPI.Contracts;

public sealed record OrderItemResponse(int BookId, string Title, int Quantity, decimal Price);
