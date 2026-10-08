namespace BookShopAppAPI.Services;

public enum CartAddOutcome { Added, BookNotFound, InsufficientStock }

public sealed record CartAddResult(CartAddOutcome Outcome, int Quantity = 0, int Stock = 0);
