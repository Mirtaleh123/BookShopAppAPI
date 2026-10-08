namespace BookShopAppAPI.Contracts;

public sealed record BookResponse(
    int Id, string Title, string Author, decimal Price, string ImageUrl,
    string About, int Stock, int CategoryId, string? CategoryName, double AverageRating);
