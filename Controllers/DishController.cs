using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[ApiController]
[Route("api/restaurant/{restaurantId}/dish")]
public class DishController : ControllerBase
{
    private readonly IDishService _dishService;

    public DishController(IDishService dishService)
    {
        _dishService = dishService ?? throw new ArgumentNullException(nameof(dishService));
    }
    
    [HttpPost]
    public ActionResult Post([FromRoute] int restaurantId, [FromBody] CreateDishDto dto)
    {
        var newDishId = _dishService.Create(restaurantId, dto);
        
        return Created($"api/restaurant/{restaurantId}/dish/{newDishId}", null);
    }
    
    [HttpGet("{dishId}")]
    public ActionResult<DishDto> Get([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        var dish = _dishService.GetById(restaurantId, dishId);
        
        return Ok(dish);
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<DishDto>> GetAll([FromRoute] int restaurantId)
    {
        var dishes = _dishService.GetAll(restaurantId);
        
        return Ok(dishes);
    }
    
    [HttpDelete("{dishId}")]
    public ActionResult Delete([FromRoute] int restaurantId, [FromRoute] int dishId)
    {
        _dishService.Delete(restaurantId, dishId);
        
        return NoContent();
    }
    
    [HttpDelete]
    public ActionResult DeleteAll([FromRoute] int restaurantId)
    {
        _dishService.DeleteAll(restaurantId);
        
        return NoContent();
    }
}