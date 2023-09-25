using System.Security.Claims;

namespace RestaurantAPI.Services;

public class UserContextService : IUserContextService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
    }
    
    public ClaimsPrincipal User => _httpContextAccessor.HttpContext?.User;
    public int? GetUserId => User is null ? null : 
                    (int?)int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
}