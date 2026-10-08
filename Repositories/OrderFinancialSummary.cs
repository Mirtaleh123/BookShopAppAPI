using BookShopAppAPI.Models;

namespace BookShopAppAPI.Repositories;

public sealed record OrderFinancialSummary(OrderStatus Status, decimal TotalPrice);
