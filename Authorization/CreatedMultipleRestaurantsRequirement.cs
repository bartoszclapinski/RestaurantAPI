using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization;

public class CreatedMultipleRestaurantsRequirement : IAuthorizationRequirement
{
    public int MinimumRestaurantsCreated { get; }

    public CreatedMultipleRestaurantsRequirement(int minimumRestaurantsCreated)
    {
        MinimumRestaurantsCreated = minimumRestaurantsCreated;
    }
}