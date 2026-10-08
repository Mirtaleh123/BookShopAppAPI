using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace BookShopAppAPI.Controllers;

public abstract class AuthenticatedControllerBase : ControllerBase
{
    protected int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
}
