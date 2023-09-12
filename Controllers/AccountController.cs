using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.Models;
using RestaurantAPI.Services;

namespace RestaurantAPI.Controllers;

[ApiController]
[Route("api/account")]
public class AccountController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountController(IAccountService accountService)
    {
        _accountService = accountService ?? throw new ArgumentNullException(nameof(accountService));
    }
    
    [HttpPost("register")]
    public ActionResult RegisterUser([FromBody] RegisterUserDto dto)
    {
        _accountService.RegisterUser(dto);
        return Ok();            
    }
    
    [HttpPost("login")]
    public ActionResult Login([FromBody] LoginDto dto)
    {
        string token = _accountService.GenerateJwt(dto);
        return Ok(token);
    }
    
}