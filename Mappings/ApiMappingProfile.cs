using AutoMapper;
using BookShopAppAPI.Contracts;
using BookShopAppAPI.Models;

namespace BookShopAppAPI.Mappings;

public sealed class ApiMappingProfile : Profile
{
    public ApiMappingProfile()
    {
        CreateMap<CreateBookRequest, Book>()
            .ForMember(book => book.Id, options => options.Ignore())
            .ForMember(book => book.IsDeleted, options => options.Ignore())
            .ForMember(book => book.Category, options => options.Ignore())
            .ForMember(book => book.BookRatings, options => options.Ignore());

        CreateMap<UpdateBookRequest, Book>()
            .ForMember(book => book.Id, options => options.Ignore())
            .ForMember(book => book.IsDeleted, options => options.Ignore())
            .ForMember(book => book.Category, options => options.Ignore())
            .ForMember(book => book.BookRatings, options => options.Ignore());

        CreateMap<Book, BookResponse>()
            .ForCtorParam(nameof(BookResponse.CategoryName), options =>
                options.MapFrom(book => book.Category == null ? null : book.Category.Name))
            .ForCtorParam(nameof(BookResponse.AverageRating), options =>
                options.MapFrom(book => book.BookRatings.Count == 0
                    ? 0d : book.BookRatings.Average(rating => rating.Rating)));

        CreateMap<Category, CategoryResponse>();

        CreateMap<AddCartItemRequest, Cart>()
            .ForMember(cart => cart.Id, options => options.Ignore())
            .ForMember(cart => cart.UserId, options => options.Ignore())
            .ForMember(cart => cart.Book, options => options.Ignore());

        CreateMap<Cart, CartItemResponse>()
            .ForCtorParam(nameof(CartItemResponse.Title), options =>
                options.MapFrom(cart => cart.Book.Title))
            .ForCtorParam(nameof(CartItemResponse.Price), options =>
                options.MapFrom(cart => cart.Book.Price))
            .ForCtorParam(nameof(CartItemResponse.Stock), options =>
                options.MapFrom(cart => cart.Book.Stock));

        CreateMap<Cart, OrderItem>()
            .ForMember(item => item.Id, options => options.Ignore())
            .ForMember(item => item.OrderId, options => options.Ignore())
            .ForMember(item => item.Order, options => options.Ignore())
            .ForMember(item => item.Book, options => options.Ignore())
            .ForMember(item => item.Price, options =>
                options.MapFrom(cart => cart.Book.Price));

        CreateMap<OrderItem, OrderItemResponse>()
            .ForCtorParam(nameof(OrderItemResponse.Title), options =>
                options.MapFrom(item => item.Book.Title));

        CreateMap<Order, OrderResponse>()
            .ForCtorParam(nameof(OrderResponse.Status), options =>
                options.MapFrom(order => order.Status.ToString()))
            .ForCtorParam(nameof(OrderResponse.Items), options =>
                options.MapFrom(order => order.OrderItems));

        CreateMap<Order, AdminOrderResponse>()
            .ForCtorParam(nameof(AdminOrderResponse.Status), options =>
                options.MapFrom(order => order.Status.ToString()))
            .ForCtorParam(nameof(AdminOrderResponse.Items), options =>
                options.MapFrom(order => order.OrderItems));

        CreateMap<BookRating, RatingResponse>()
            .ForCtorParam(nameof(RatingResponse.BookTitle), options =>
                options.MapFrom(rating => rating.Book.Title))
            .ForCtorParam(nameof(RatingResponse.Username), options =>
                options.MapFrom(rating => rating.User.Username));

        CreateMap<RegisterRequest, AppUser>()
            .ForMember(user => user.Id, options => options.Ignore())
            .ForMember(user => user.PasswordHash, options => options.Ignore())
            .ForMember(user => user.Role, options => options.Ignore())
            .ForMember(user => user.BookRatings, options => options.Ignore());

        CreateMap<AppUser, RegisteredUserResponse>();
        CreateMap<AppUser, LoginUserResponse>();
    }
}
