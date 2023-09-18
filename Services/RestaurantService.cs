using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Authorization;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entities;
using RestaurantAPI.Excepctions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public class RestaurantService : IRestaurantService
{
    private readonly RestaurantDbContext _dbContext;
    private readonly IMapper _mapper;
    private readonly ILogger<RestaurantService> _logger;
    private readonly IAuthorizationService _authorizationService;
    private IRestaurantService _restaurantServiceImplementation;

    public RestaurantService(
                    RestaurantDbContext dbContext, 
                    IMapper mapper, 
                    ILogger<RestaurantService> logger,
                    IAuthorizationService authorizationService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
    }
    
    public RestaurantDto GetById(int id)
    {
        var restaurant = _dbContext.Restaurants
            .Include(r => r.Address)
            .Include(r => r.Dishes)
            .FirstOrDefault(r => r.Id == id);
        
        if (restaurant is null) throw new NotFoundException("Restaurant not found.");
        
        var result = _mapper.Map<RestaurantDto>(restaurant);

        return result;
    }

    public IEnumerable<RestaurantDto> GetAll()
    {
        var restaurants = _dbContext.Restaurants
                        .Include(r => r.Address)
                        .Include(r => r.Dishes)
                        .ToList();
        
        var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);
        
        return restaurantsDto;
    }
    
    public int Create(CreateRestaurantDto dto, int userId)
    {
        var restaurant = _mapper.Map<Restaurant>(dto);
        restaurant.CreatedById = userId;
        _dbContext.Restaurants.Add(restaurant);
        _dbContext.SaveChanges();

        return restaurant.Id;
    }
    
    public void Delete(int id, ClaimsPrincipal user)
    {
        _logger.LogError("Restaurant with id: {Id} DELETE action invoked", id);
        
        var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);
        if (restaurant is null) throw new NotFoundException("Restaurant not found.");
        
        var authorizationResult = _authorizationService
                        .AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete))
                        .Result;
        
        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException();
        }
        
        _dbContext.Restaurants.Remove(restaurant);
        _dbContext.SaveChanges();
        
    }

    public void Update(int id, UpdateRestaurantDto dto, ClaimsPrincipal user)
    {
        var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);
        if (restaurant is null) throw new NotFoundException("Restaurant not found.");

        var authorizationResult = _authorizationService
                        .AuthorizeAsync(user, restaurant, new ResourceOperationRequirement(ResourceOperation.Update))
                        .Result;
        
        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException();
        }
        
        restaurant.Name = dto.Name;
        restaurant.Description = dto.Description;
        restaurant.HasDelivery = dto.HasDelivery;
        
        _dbContext.SaveChanges();
    }
}