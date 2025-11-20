using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskManager.Api.Data;
using TaskManager.Api.DTOs;
using TaskManager.Api.Services;

namespace TaskManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly ITokenService _tokenService;

        public AuthController(AppDbContext db, ITokenService tokenService)
        {
            _db = db;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest req)
        {
            var user = await _db.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Username == req.Username);
            if (user == null) return Unauthorized("Invalid credentials");

            var verified = BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash);
            if (!verified) return Unauthorized("Invalid credentials");

            var token = _tokenService.CreateToken(user, user.Role?.Name ?? "User");

            return Ok(new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role?.Name ?? "User"
            });
        }
    }
}
