using System.ComponentModel.DataAnnotations;

namespace RestaurantAPI.Controllers;

public class RegisterUserDto
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Nationality { get; set; }
    public DateTime? DateOfBirth  { get; set; }

    public int RoleId { get; set; } = 1;
}