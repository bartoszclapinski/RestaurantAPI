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
    private readonly IUserContextService _userContextService;
    private IRestaurantService _restaurantServiceImplementation;

    public RestaurantService(
                    RestaurantDbContext dbContext, 
                    IMapper mapper, 
                    ILogger<RestaurantService> logger,
                    IAuthorizationService authorizationService,
                    IUserContextService userContextService)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _authorizationService = authorizationService ?? throw new ArgumentNullException(nameof(authorizationService));
        _userContextService = userContextService ?? throw new ArgumentNullException(nameof(userContextService));
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

    public PagedResult<RestaurantDto> GetAll(RestaurantQuery query)
    {
        var baseQuery = _dbContext.Restaurants
                        .Include(r => r.Address)
                        .Include(r => r.Dishes)
                        .Where(r => query.searchPhrase == null || (r.Name.ToLower().Contains(query.searchPhrase.ToLower()) || r.Description.ToLower().Contains(query.searchPhrase.ToLower())));
        
        var restaurants = baseQuery
                        .Skip(query.PageSize * (query.PageNumber - 1))
                        .Take(query.PageSize)
                        .ToList();
        
        var totalItemsCount = baseQuery.Count();
        
        var restaurantsDto = _mapper.Map<List<RestaurantDto>>(restaurants);

        var result = new PagedResult<RestaurantDto>(restaurantsDto, totalItemsCount, query.PageSize, query.PageNumber);
        
        return result;
    }
    
    public int Create(CreateRestaurantDto dto)
    {
        var restaurant = _mapper.Map<Restaurant>(dto);
        restaurant.CreatedById = _userContextService.GetUserId;
        _dbContext.Restaurants.Add(restaurant);
        _dbContext.SaveChanges();

        return restaurant.Id;
    }
    
    public void Delete(int id)
    {
        _logger.LogError("Restaurant with id: {Id} DELETE action invoked", id);
        
        var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);
        if (restaurant is null) throw new NotFoundException("Restaurant not found.");
        
        var authorizationResult = _authorizationService
                        .AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete))
                        .Result;
        
        if (!authorizationResult.Succeeded)
        {
            throw new ForbidException();
        }
        
        _dbContext.Restaurants.Remove(restaurant);
        _dbContext.SaveChanges();
        
    }

    public void Update(int id, UpdateRestaurantDto dto)
    {
        var restaurant = _dbContext.Restaurants.FirstOrDefault(r => r.Id == id);
        if (restaurant is null) throw new NotFoundException("Restaurant not found.");

        var authorizationResult = _authorizationService
                        .AuthorizeAsync(_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update))
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