using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Controllers;
using RestaurantAPI.Entities;
using RestaurantAPI.Excepctions;
using RestaurantAPI.Models;

namespace RestaurantAPI.Services;

public class AccountService : IAccountService
{
    private readonly RestaurantDbContext _context;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly AuthenticationSettings _authenticationSettings;

    public AccountService(
                    RestaurantDbContext context, 
                    IPasswordHasher<User> passwordHasher,
                    AuthenticationSettings authenticationSettings)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        _passwordHasher = passwordHasher ?? throw new ArgumentNullException(nameof(passwordHasher));
        _authenticationSettings = authenticationSettings ?? throw new ArgumentNullException(nameof(authenticationSettings));
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
    
    public string GenerateJwt(LoginDto dto)
    {
        var user = _context.Users
                        .Include(u => u.Role)
                        .FirstOrDefault(u => u.Email == dto.Email);
        if (user is null)
        {
            throw new BadRequestException("Invalid username or password.");
        }

        var passwordVerificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
        if (passwordVerificationResult == PasswordVerificationResult.Failed)
        {
            throw new BadRequestException("Invalid username or password.");
        }
        
        var claims = new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
            new Claim(ClaimTypes.Role, user.Role.Name),
            new Claim("DateOfBirth", user.DateOfBirth?.ToString("yyyy-MM-dd") ?? string.Empty)
        };

        if (!string.IsNullOrEmpty(user.Nationality))
        {
            claims.Add(new Claim("Nationality", user.Nationality));
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationSettings.JwtKey));
        var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authenticationSettings.JwtExpireDays);
        
        var token = new JwtSecurityToken(
            _authenticationSettings.JwtIssuer,
            _authenticationSettings.JwtIssuer,
            claims,
            expires: expires,
            signingCredentials: cred
        );
        
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
        
        
    }
} 