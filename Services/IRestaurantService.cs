using System.Security.Claims;
using RestaurantAPI.Controllers;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public interface IRestaurantService
{
    RestaurantDto GetById(int id);
    PagedResult<RestaurantDto> GetAll(RestaurantQuery query);
    int Create(CreateRestaurantDto dto);
    void Delete(int id);
    void Update(int id, UpdateRestaurantDto dto);
}