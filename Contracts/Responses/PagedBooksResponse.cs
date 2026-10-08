namespace BookShopAppAPI.Contracts;

public sealed record PagedBooksResponse(
    IReadOnlyList<BookResponse> Items, int TotalCount, int Page, int PageSize, int TotalPages);
