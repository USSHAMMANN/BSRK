using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.Json;
using ToDoList.Models;
using ToDoList.API;
using System.Globalization;
using Microsoft.AspNetCore.Authentication;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ToDoList.Controllers
{
    public class UserController : Controller
    {
        private readonly ApiService _apiService;
        
        
        public UserController(IHttpContextAccessor httpContextAccessor)
        {
            _apiService = new ApiService(httpContextAccessor);
        }

        public IActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RegistrationUser(User user)
        {
            if (user == null)
            {
                return View("Error");
            }
            await _apiService.RegisterUserAsync(user);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Authorization()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AuthorizationUser(User user)
        {
            if (user == null)
            {
                return View("Error");
            }
            
            if (await LoginUser(user))
                return RedirectToAction("Index", "Home");

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Удаление куки с токеном
            Response.Cookies.Delete("token");

            return RedirectToAction("Index", "Home");
        }

        private async Task<bool> LoginUser(User user)
        {
            string token = await _apiService.AuthorizationUserApi(user);
            if (!string.IsNullOrEmpty(token))
            {
                SetCookieJwt(token);

                var _token = new JwtSecurityTokenHandler().ReadJwtToken(token);
                var claims = _token.Claims;
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(principal);

                Console.WriteLine(token);
                
                return true;
            }
            return false;
        }

        private void SetCookieJwt(string token)
        {
            HttpContext.Response.Cookies.Append("token", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Strict
            });
        }

      
       
    }
}
