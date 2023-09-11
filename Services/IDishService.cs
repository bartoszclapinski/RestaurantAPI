using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public interface IDishService
{
    int Create(int restaurantId, CreateDishDto dto);
    DishDto GetById(int restaurantId, int dishId);
    List<DishDto> GetAll(int restaurantId);
    void Delete(int restaurantId, int dishId);
    void DeleteAll(int restaurantId);
}