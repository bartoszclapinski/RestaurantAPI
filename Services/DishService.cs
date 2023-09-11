using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Excepctions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public class DishService : IDishService
{
    private readonly RestaurantDbContext _context;
    private readonly IMapper _mapper;

    public DishService(RestaurantDbContext context, IMapper mapper)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    public int Create(int restaurantId, CreateDishDto dto)
    {
        var restaurant = GetRestaurantById(restaurantId);
        
        var dishEntity = _mapper.Map<Dish>(dto);
        dishEntity.RestaurantId = restaurantId;
        
        _context.Dishes.Add(dishEntity);
        _context.SaveChanges();
        
        return dishEntity.Id;
    }

    public DishDto GetById(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurantById(restaurantId);
        
        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
        if (dish is null || dish.RestaurantId != restaurantId) throw new NotFoundException("Dish not found.");
        
        var dishDto = _mapper.Map<DishDto>(dish);
        
        return dishDto;
    }
    
    public List<DishDto> GetAll(int restaurantId)
    {
        var restaurant = GetRestaurantById(restaurantId);
        
        var dishesDto = _mapper.Map<List<DishDto>>(restaurant.Dishes);
        
        return dishesDto;
    }

    public void Delete(int restaurantId, int dishId)
    {
        var restaurant = GetRestaurantById(restaurantId);
        
        var dish = restaurant.Dishes.FirstOrDefault(d => d.Id == dishId);
        if (dish is null || dish.RestaurantId != restaurantId) throw new NotFoundException("Dish not found.");
        
        _context.Dishes.Remove(dish);
        _context.SaveChanges();
    }

    public void DeleteAll(int restaurantId)
    {
        var restaurant = GetRestaurantById(restaurantId);
        
        _context.RemoveRange(restaurant.Dishes);
        _context.SaveChanges();
    }
    
    private Restaurant GetRestaurantById(int restaurantId)
    {
        var restaurant = _context.Restaurants
                        .Include(r => r.Dishes)
                        .FirstOrDefault(r => r.Id == restaurantId);
        if (restaurant is null) throw new NotFoundException("Restaurant not found.");
        
        return restaurant;
    }
}