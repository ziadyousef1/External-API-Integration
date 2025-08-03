using External_API_Integration.Data;
using External_API_Integration.DTOs;
using External_API_Integration.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace External_API_Integration.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AccountController(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto model)
        {
            // 1. we need to verify the user credentials
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username); // use any first as a test for them 

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
            {
                return Unauthorized("Invalid username or password");
            }

            // 2. the user is authenticated, we need to generate a token
            var token = GenrateToken(user);

            // 3. return the token to the user
            return Ok(new
            {
                Token = token
                //User = new { user.Id, user.Username, user.Email, user.Role }
            });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto model)
        {
            // 1. check if the use already ecists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == model.Username || u.Email == model.Email);

            if (existingUser != null)
            {
                return BadRequest("User already exists");
            }

            // 2. create a new user -> we need to hash the password before saving to db
            var newUser = new User();
            newUser.Username = model.Username;
            newUser.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            newUser.Email = model.Email;

            if (model.Role == null || model.Role.Trim() == string.Empty)
                newUser.Role = "USER"; // default role if not provided
            else if (model.Role != "ADMIN" && model.Role != "USER")
                return BadRequest("Invalid role. Only 'ADMIN' or 'USER' are allowed.");

            newUser.Role = model.Role;

            // TODO: confirm the mail first

            // 3. add the new user to the database
            _context.Users.Add(newUser);
            await _context.SaveChangesAsync();

            return Ok(newUser);
        }

        private string GenrateToken(User user)
        {
            // 1. create a security key
            var jwtKey = _configuration["Jwt:Key"];
            if (string.IsNullOrEmpty(jwtKey))
                throw new InvalidOperationException("JWT Key is not configured");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            // 2. create a signing credentials
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // 3. Claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            };

            // 4. create a token 
            var token = new JwtSecurityToken(
                    // issuer: _configuration["Jwt:Issuer"],
                    // audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
