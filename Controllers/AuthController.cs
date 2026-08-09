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

            if (IsAdminToken(token.AccessToken))
            {
                Response.Cookies.Append("IsAdmin", "true", new CookieOptions
                {
                    Secure = true,
                    Expires = token.ExpiryTime
                });
            }

            return RedirectToAction("Index", "Home");
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
                var error = await response.Content.ReadAsStringAsync();
                ViewData["Error"] = string.IsNullOrWhiteSpace(error)
                    ? "Registration failed. Please check your details and try again."
                    : error;
                return View(signUpRequest);
            }

            var loginResponse = await client.PostAsJsonAsync("/api/Auth/login", new SignInRequest
            {
                Email = signUpRequest.Email,
                Password = signUpRequest.Password
            });

            if (!loginResponse.IsSuccessStatusCode)
            {
                TempData["Success"] = "Account created! You can now sign in.";
                return RedirectToAction("Login");
            }

            var token = await loginResponse.Content.ReadFromJsonAsync<TokenResponse>();
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

            if (IsAdminToken(token.AccessToken))
            {
                Response.Cookies.Append("IsAdmin", "true", new CookieOptions
                {
                    Secure = true,
                    Expires = token.ExpiryTime
                });
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Delete("IsAdmin");
            return RedirectToAction("Login");
        }

        private static bool IsAdminToken(string token)
        {
            try
            {
                var payload = token.Split('.')[1];
                var padded = payload.PadRight(payload.Length + (4 - payload.Length % 4) % 4, '=');
                var json = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(padded));
                return json.Contains("\"Admin\"");
            }
            catch { return false; }
        }
    }
}
