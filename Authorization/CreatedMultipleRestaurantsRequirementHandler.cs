using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Authorization;

public class CreatedMultipleRestaurantsRequirementHandler : AuthorizationHandler<CreatedMultipleRestaurantsRequirement>
{
    private readonly RestaurantDbContext _dbContext;

    public CreatedMultipleRestaurantsRequirementHandler(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CreatedMultipleRestaurantsRequirement requirement)
    {
        var userId = int.Parse(context.User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
        var count = _dbContext.Restaurants.Count(r => r.CreatedById == userId);

        if (count >= requirement.MinimumRestaurantsCreated)
        {
            context.Succeed(requirement);
        }

        return Task.CompletedTask;    
    }
}