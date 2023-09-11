using Microsoft.EntityFrameworkCore.Metadata.Internal;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Services;

public class RestaurantSeeder
{
    private readonly RestaurantDbContext _dbContext;

    public RestaurantSeeder(RestaurantDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Seed()
    {
        if (!_dbContext.Database.CanConnect()) return;
        
        if (!_dbContext.Roles.Any())
        {
            var roles = GetRoles();
            _dbContext.Roles.AddRange(roles);
            _dbContext.SaveChanges();
        }

        if (!_dbContext.Restaurants.Any())
        {
            var restaurants = GetRestaurants();
            _dbContext.Restaurants.AddRange(restaurants);
            _dbContext.SaveChanges();
        }
    }
    
    private IEnumerable<Role> GetRoles()
    {
        var roles = new List<Role>
        {
            new() { Name = "User" },
            new() { Name = "Manager" },
            new() { Name = "Admin" }
        };
        return roles;
    }

    private IEnumerable<Restaurant> GetRestaurants()
    {
        var restaurants = new List<Restaurant>
        {
            new()
            {
                Name = "KFC",
                Category = "Fast Food",
                Description ="KFC (short for Kentucky Fried Chicken) is an American fast food restaurant chain headquartered in Louisville, Kentucky, that specializes in fried chicken. It is the world's second-largest restaurant chain (as measured by sales) after McDonald's, with 22,621 locations globally in 150 countries as of December 2019. The chain is a subsidiary of Yum! Brands, a restaurant company that also owns the Pizza Hut, Taco Bell, and WingStreet chains.",
                ContactEmail = "contact@kfc.com",
                ContactNumber = "11-22-33",
                HasDelivery = true,
                Dishes = new List<Dish>
                {
                    new()
                    {
                        Name = "Chicken wings",
                        Description = "Chicken wings",
                        Price = 7.99M
                    },
                    new()
                    {
                        Name = "Chicken burger",
                        Description = "Chicken burger",
                        Price = 6.99M
                    }
                },
                Address = new Address
                {
                    City = "Cracow",
                    Street = "Długa 5",
                    PostalCode = "30-001"
                }
            },
            new()
            {
                Name = "McDonald's",
                Category = "Fast Food",
                Description ="McDonald's Corporation is an American fast food company, founded in 1940 as a restaurant operated by Richard and Maurice McDonald, in San Bernardino, California, United States. They rechristened their business as a hamburger stand, and later turned the company into a franchise, with the Golden Arches logo being introduced in 1953 at a location in Phoenix, Arizona. In 1955, Ray Kroc, a businessman, joined the company as a franchise agent and proceeded to purchase the chain from the McDonald brothers. McDonald's had its previous headquarters in Oak Brook, Illinois, but moved its global headquarters to Chicago in June 2018.",
                ContactEmail = "contact@mcdonald.com",
                ContactNumber = "12-34-56",
                HasDelivery = false,
                Dishes = new List<Dish>
                {
                    new()
                    {
                        Name = "Big Mac",
                        Description = "Big Mac",
                        Price = 5.99M
                    },
                    new()
                    {
                        Name = "French fries",
                        Description = "French fries",
                        Price = 2.99M
                    }
                },
                Address = new Address()
                {
                    City = "Cracow",
                    Street = "Długa 15",
                    PostalCode = "30-001"
                }
            }
        };
        return restaurants;
    }
}