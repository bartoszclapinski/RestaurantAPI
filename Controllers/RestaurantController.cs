using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[ApiController]
[Route("api/restaurant")]
public class RestaurantController : ControllerBase
{
    private readonly IRestaurantService _restaurantService;

    public RestaurantController(IRestaurantService restaurantService)
    {
        _restaurantService = restaurantService ?? throw new ArgumentNullException(nameof(restaurantService));
    }
    
    [HttpGet]
    public ActionResult<IEnumerable<RestaurantDto>> GetAll()
    {
        var restaurantsDto = _restaurantService.GetAll();
        
        return Ok(restaurantsDto);   
    }
    
    [HttpGet("{id}")]
    public ActionResult<RestaurantDto> Get([FromRoute] int id)
    {
        var restaurantDto = _restaurantService.GetById(id);
        
        return Ok(restaurantDto);
    }
    
    [HttpPost]
    public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
    {
        var id = _restaurantService.Create(dto);
        
        return Created($"/api/restaurant/{id}", null);
    }
    
    [HttpDelete("{id}")]
    public ActionResult DeleteRestaurant([FromRoute] int id)
    {
        _restaurantService.Delete(id);
        
        return NoContent();
    }
    
    [HttpPut("{id}")]
    public ActionResult UpdateRestaurant([FromRoute] int id, [FromBody] UpdateRestaurantDto dto)
    {
        _restaurantService.Update(id, dto);
        
        return Ok();
    }
}