namespace BookShopAppAPI.Contracts;

public sealed record DashboardResponse(int TotalOrders, int TotalBooks, int PendingOrders, decimal TotalRevenue);
