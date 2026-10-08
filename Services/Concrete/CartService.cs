using BookShopAppAPI.Services.Abstract;
using BookShopAppAPI.Services.Base;
using BookShopAppAPI.Services;
using AutoMapper;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;
using BookShopAppAPI.Repositories.Abstract;

namespace BookShopAppAPI.Services.Concrete;

public sealed class CartService(ICartRepository cart, IMapper mapper)
    : BaseService<ICartRepository>(cart), ICartService
{
    public async Task<CartResponse> GetUserCartAsync(int userId)
    {
        var items = await Repository.GetUserItemsAsync(userId);
        return new CartResponse(
            mapper.Map<List<CartItemResponse>>(items),
            items.Sum(item => item.Book.Price * item.Quantity));
    }

    public async Task<CartAddResult> AddAsync(int userId, AddCartItemRequest request)
    {
        var book = await Repository.GetBookAsync(request.BookId);
        if (book is null) return new(CartAddOutcome.BookNotFound);

        var existing = await Repository.GetItemAsync(userId, request.BookId);
        var newQuantity = (existing?.Quantity ?? 0) + request.Quantity;
        if (newQuantity > book.Stock)
            return new(CartAddOutcome.InsufficientStock, Stock: book.Stock);

        if (existing is null)
        {
            var item = mapper.Map<Cart>(request);
            item.UserId = userId;
            await Repository.AddAsync(item);
        }
        else
            existing.Quantity = newQuantity;

        await SaveAsync();
        return new(CartAddOutcome.Added, Quantity: newQuantity);
    }

    public async Task<bool> RemoveAsync(int userId, int itemId)
    {
        var item = await Repository.GetItemByIdAsync(itemId, userId);
        if (item is null) return false;
        Repository.Remove(item);
        await SaveAsync();
        return true;
    }
}
