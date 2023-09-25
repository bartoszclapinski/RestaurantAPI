using System.Security.Claims;

namespace RestaurantAPI.Services;

public interface IUserContextService
{
    ClaimsPrincipal User { get; }
    int? GetUserId { get; }
}