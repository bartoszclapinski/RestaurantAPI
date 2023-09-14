using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Authorization;

public class MinimumAgeRequirementHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    private readonly ILogger<MinimumAgeRequirementHandler> _logger;

    public MinimumAgeRequirementHandler(ILogger<MinimumAgeRequirementHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }
    
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var dateOfBirth = 
                        DateTime.Parse(context.User.FindFirst(c => c.Type == "DateOfBirth")?.Value);
        
        var userEmail = context.User.FindFirst(c => c.Type == ClaimTypes.Name)?.Value;
        _logger.LogInformation($"User {userEmail} with date of birth: [{dateOfBirth}]");
        
        if (dateOfBirth.AddYears(requirement.MinimumAge) <= DateTime.Today)
        {
            _logger.LogInformation("Authorization succeeded");
            context.Succeed(requirement);
        }
        else
        {
            _logger.LogInformation("Authorization failed");
        }
        
        return Task.CompletedTask;
    }
}