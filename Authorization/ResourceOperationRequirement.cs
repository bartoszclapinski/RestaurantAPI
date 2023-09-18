using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization;

public class ResourceOperationRequirement : IAuthorizationRequirement
{
    public ResourceOperationRequirement(ResourceOperation operation)
    {
        Operation = operation;
    }
    
    public ResourceOperation Operation { get; }
}