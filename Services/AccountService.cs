using Microsoft.AspNetCore.Identity;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entities;

namespace RestaurantAPI.Services;

public class AccountService : IAccountService
{
    private readonly RestaurantDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;

    public AccountService(RestaurantDbContext context, IPasswordHasher<User> passwordHasher)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
    }
    
    public void RegisterUser(RegisterUserDto dto)
    {
        var newUser = new User()
        {
                        Email = dto.Email,
                        DateOfBirth = dto.DateOfBirth,
                        Nationality = dto.Nationality,
                        FirstName = dto.FirstName,
                        LastName = dto.LastName,
                        RoleId = dto.RoleId
        };
        
        var hashPassword = _passwordHasher.HashPassword(newUser, dto.Password);
        newUser.PasswordHash = hashPassword;
        _context.Users.Add(newUser);
        _context.SaveChanges();
    }
} 