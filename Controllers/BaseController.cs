using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;

namespace TournamentManager.Frontend.Controllers
{
    public abstract class BaseController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        protected BaseController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected HttpClient? CreateAuthorizedClient(out IActionResult? redirect)
        {
            var token = Request.Cookies["AccessToken"];
            if (token is null)
            {
                redirect = ClearAndRedirectToLogin();
                return null;
            }

            var client = _httpClientFactory.CreateClient("TournamentManagerApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            redirect = null;
            return client;
        }

        protected IActionResult? CheckUnauthorized(HttpResponseMessage response)
        {
            if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                return ClearAndRedirectToLogin();
            return null;
        }

        protected IActionResult? CheckAdminAccess()
        {
            var token = Request.Cookies["AccessToken"];
            if (string.IsNullOrEmpty(token) || !IsAdminJwt(token))
                return RedirectToAction("Index", "Home");
            return null;
        }

        private static bool IsAdminJwt(string token)
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

        private IActionResult ClearAndRedirectToLogin()
        {
            Response.Cookies.Delete("AccessToken");
            Response.Cookies.Delete("RefreshToken");
            Response.Cookies.Delete("IsAdmin");
            return RedirectToAction("Login", "Auth");
        }
    }
}
