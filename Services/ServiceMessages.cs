namespace BookShopAppAPI.Services;

public static class ServiceMessages
{
    public const string EmptyCart = "Səbət boşdur.";
    public const string OrderNotFound = "Sifariş tapılmadı.";
    public const string OrderNotPending = "Yalnız gözləyən sifarişi ləğv etmək olar.";

    public static string InsufficientStock(string title) =>
        $"{title} üçün kifayət qədər stok yoxdur.";
}
