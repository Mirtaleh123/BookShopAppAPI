namespace BookShopAppAPI.Contracts;

public sealed record CategoryResponse(int Id, string Name, IReadOnlyList<BookResponse> Books);
