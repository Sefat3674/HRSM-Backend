using BCrypt.Net;
using HRMS.API.DTOs;
using HRMS.DAL.Data;
using HRMS.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HRMS.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly HRMSDbContext _context;

        public AdminController(HRMSDbContext context)
        {
            _context = context;
        }

        // POST: api/admin/create-user
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] RegisterDto dto)
        {
            // Validate role exists
            var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == dto.RoleId);
            if (role == null)
                return BadRequest(new { message = "Invalid RoleId" });

            // Check if username already exists
            if (await _context.Users.AnyAsync(u => u.UserName == dto.UserName))
                return BadRequest(new { message = "Username already exists" });
                
            // Check if email already exists
            if (await _context.UserProfile.AnyAsync(p => p.Email == dto.Email))
                return BadRequest(new { message = "Email already exists" });

            // ✅ Hash password using BCrypt
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Create new user
            var newUser = new Users
            {
                UserName = dto.UserName,
                PasswordHash = hashedPassword,
                RoleId = role.RoleId,
                IsActive = true,
                CreateAt = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            // Create user profile
            var newProfile = new UserProfile
            {
                UserId = newUser.UserId,
                FullName = dto.FullName,
                Email = dto.Email,
                Phone = dto.Phone
            };

            _context.UserProfile.Add(newProfile);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "User created successfully",
                userId = newUser.UserId,
                userName = newUser.UserName,
                fullName = newProfile.FullName,
                email = newProfile.Email
            });
        }
        [HttpGet("get-user/{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            return Ok(new
            {
                userId = user.UserId,
                userName = user.UserName,
                roleId = user.RoleId,
                isActive = user.IsActive,
                fullName = user.UserProfile?.FullName,
                email = user.UserProfile?.Email,
                phone = user.UserProfile?.Phone
            });
        }
        [HttpPut("update-user/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _context.Users
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (user == null)
                return NotFound(new { message = "User not found" });

            // Update Role if provided
            if (dto.RoleId.HasValue && dto.RoleId.Value != user.RoleId)
            {
                var role = await _context.Roles.FirstOrDefaultAsync(r => r.RoleId == dto.RoleId.Value);
                if (role == null) return BadRequest(new { message = "Invalid RoleId" });
                user.RoleId = role.RoleId;
            }

            // Update other fields
            if (!string.IsNullOrEmpty(dto.UserName)) user.UserName = dto.UserName;
            if (!string.IsNullOrEmpty(dto.Password)) user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
            if (dto.IsActive.HasValue) user.IsActive = dto.IsActive.Value;

            var profile = user.UserProfile;
            if (profile != null)
            {
                if (!string.IsNullOrEmpty(dto.FullName)) profile.FullName = dto.FullName;
                if (!string.IsNullOrEmpty(dto.Email)) profile.Email = dto.Email;
                if (!string.IsNullOrEmpty(dto.Phone)) profile.Phone = dto.Phone;
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "User updated successfully",
                userId = user.UserId,
                userName = user.UserName,
                fullName = profile?.FullName,
                isActive = user.IsActive,
                email = profile?.Email,
                phone = profile?.Phone,
                roleId = user.RoleId
            });
        }
        
      
        // POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _context.Users
                .Include(u => u.Role)
                .Include(u => u.UserProfile)
                .FirstOrDefaultAsync(u => u.UserName == dto.UserName);

            if (user == null)
                return Unauthorized(new { message = "Invalid username" });

            // Verify password using BCrypt
            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return Unauthorized(new { message = "Invalid password" });

            if (!user.IsActive)
                return Unauthorized(new { message = "Account is inactive" });

            // Generate a temporary token (replace with JWT in production)
            var token = Guid.NewGuid().ToString();

            // Return role info so Angular can redirect
            return Ok(new
            {
                userId = user.UserId,
                userName = user.UserName,
                fullName = user.UserProfile.FullName,
                email = user.UserProfile.Email,
                roleName = user.Role?.RoleName ?? "User",
                phone=user.UserProfile.Phone,
                token
            });
        }
        

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            return Ok(new { message = "Logged out successfully" });
        }
    }
}