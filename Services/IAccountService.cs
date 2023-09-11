using RestaurantAPI.Controllers;

namespace RestaurantAPI.Services;

public interface IAccountService
{
    void RegisterUser(RegisterUserDto dto);
}