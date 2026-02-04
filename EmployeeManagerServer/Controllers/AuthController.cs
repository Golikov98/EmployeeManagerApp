using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EmployeeManagerServer.Data;
using EmployeeManagerServer.Models;
using BCrypt.Net;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _db;

    public AuthController(AppDbContext db)
    {
        _db = db;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // Находим пользователя по email
        var user = await _db.Users
            .Include(u => u.Employee)
            .FirstOrDefaultAsync(u => u.Email == request.Email);

        if (user == null)
            return Unauthorized(new { message = "Неверный email или пароль" });

        if (user.PasswordHash != BCrypt.Net.BCrypt.HashPassword(request.Password))
            return Unauthorized(new { message = "Неверный email или пароль" });

        // Возвращаем данные пользователя и сотрудника
        var response = new
        {
            user.Id,
            user.Email,
            user.AccessRights,
            Employee = new
            {
                user.Employee.Id,
                user.Employee.Name,
                user.Employee.Position,
                user.Employee.Department
            }
        };

        return Ok(response);
    }
}

public class LoginRequest
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}
