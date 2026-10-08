namespace BookShopAppAPI.Contracts;

public sealed record RatingResponse(
    int Id, int BookId, string BookTitle, int UserId, string Username,
    int Rating, DateTime CreatedAt, DateTime UpdatedAt);
