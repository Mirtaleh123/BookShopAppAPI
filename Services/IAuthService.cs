using BookShopAppAPI.Contracts;

namespace BookShopAppAPI.Services;

public interface IAuthService
{
    Task<RegisterResult> RegisterAsync(RegisterRequest request);
    Task<LoginUserResponse?> AuthenticateAsync(LoginRequest request);
}
