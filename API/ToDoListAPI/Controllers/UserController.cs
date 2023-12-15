using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ToDoListAPI.Models;


namespace ToDoListAPI.Controllers
{
    [ApiController]
    public class UserController : ControllerBase
    {

        private readonly IConfiguration _config;

        public UserController(IConfiguration config)
        {
            _config = config;
        }

        [Route("api/registr")]
        [Produces("application/json")]
        [HttpPost]
        public ActionResult RegistrationUser([FromBody] User user)
        {
            user.Id = TodolistContext._context.Users.Max(x => x.Id) + 1;
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            TodolistContext._context.Add(user);
            TodolistContext._context.SaveChanges();
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("api/login")]
        public IActionResult Login(User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var storedUser = TodolistContext._context.Users.FirstOrDefault(x => x.UserName == user.UserName);
            if (storedUser != null && BCrypt.Net.BCrypt.Verify(user.PasswordHash, storedUser.PasswordHash))
            {
                var token = GenerateJwtToken(storedUser);
                return Ok(token);
            }

            return Unauthorized(new { Message = "Invalid credentials" });
        }


        [Authorize]
        [HttpGet("api/user")]
        public async Task<ActionResult<User>> GetCurrentUser()
        {
            User user = GetCurrectUser();
            
            if (user==null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [NonAction]
        public User GetCurrectUser()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    Id = Convert.ToInt32(userClaims.FirstOrDefault(o => o.Type == ClaimTypes.NameIdentifier)?.Value),
                    UserName = userClaims.FirstOrDefault(o => o.Type == ClaimTypes.GivenName)?.Value,

                };
            }
            return null;
        }

        [NonAction]
        private string GenerateJwtToken(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config.GetSection("Jwt:Key").Value));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), // Преобразование user.Id в строку
                new Claim(ClaimTypes.GivenName, user.UserName)
            };
            var token = new JwtSecurityToken(
                _config.GetSection("Jwt:Issuer").Value, _config.GetSection("Jwt:Audience").Value,
                claims,
                expires: DateTime.Now.AddMinutes(59),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
