using BookShopAppAPI.Contracts;

namespace BookShopAppAPI.Services;

public enum RegisterOutcome { Created, UsernameExists, EmailExists }

public sealed record RegisterResult(RegisterOutcome Outcome, RegisteredUserResponse? User = null);
