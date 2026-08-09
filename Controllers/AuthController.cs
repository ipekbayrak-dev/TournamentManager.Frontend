using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Auth;

namespace TournamentManager.Frontend.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(SignInRequest signInRequest)
        {
            var client = _httpClientFactory.CreateClient("TournamentManagerApi");
            var response = await client.PostAsJsonAsync("/api/Auth/login", signInRequest);

            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = "Invalid email or password.";
                return View(signInRequest);
            }

            var token = await response.Content.ReadFromJsonAsync<TokenResponse>();
            Response.Cookies.Append("AccessToken", token!.AccessToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = token.ExpiryTime
            });
            Response.Cookies.Append("RefreshToken", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });

            return RedirectToAction("Profile", "Player");
        }

        [HttpGet]
        public IActionResult Register() => View();

        [HttpPost]
        public async Task<IActionResult> Register(SignUpRequest signUpRequest)
        {
            var client = _httpClientFactory.CreateClient("TournamentManagerApi");
            var response = await client.PostAsJsonAsync("/api/Auth/register", signUpRequest);

            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = "Registration failed. Please check your details and try again.";
                return View(signUpRequest);
            }

            TempData["Success"] = "Account created! You can now sign in.";
            return RedirectToAction("Login");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            return RedirectToAction("Login");
        }
    }
}
