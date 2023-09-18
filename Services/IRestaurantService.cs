using System.Security.Claims;
using RestaurantAPI.Controllers;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public interface IRestaurantService
{
    RestaurantDto GetById(int id);
    IEnumerable<RestaurantDto> GetAll();
    int Create(CreateRestaurantDto dto, int userId);
    void Delete(int id, ClaimsPrincipal user);
    void Update(int id, UpdateRestaurantDto dto, ClaimsPrincipal user);
}