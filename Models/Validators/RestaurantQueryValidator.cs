using FluentValidation;

namespace RestaurantAPI.Models.Validators;

public class RestaurantQueryValidator : AbstractValidator<RestaurantQuery>
{
    private int[] allowedPageSizes = new[] { 5, 10, 15 };
    private string[] allowedSortByColumnNames = 
                    { nameof(RestaurantDto.Name), nameof(RestaurantDto.Category), nameof(RestaurantDto.Description) };
    public RestaurantQueryValidator()
    {
        RuleFor(x => x.PageNumber).GreaterThanOrEqualTo(1);
        RuleFor(r => r.PageSize).Custom((value, context) =>
        {
            if (!allowedPageSizes.Contains(value))
            {
                context.AddFailure("PageSize", $"PageSize must in [{string.Join(",", allowedPageSizes)}]");
            }
        });
        RuleFor(r => r.SortBy)
                        .Must(value => string.IsNullOrEmpty(value) || allowedSortByColumnNames.Contains(value))
                        .WithMessage($"Sort by is optional, or must be in [{string.Join(",", allowedSortByColumnNames)}]");
    }
}