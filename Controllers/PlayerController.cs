using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Player;
using TournamentManager.Frontend.Models.Team;

namespace TournamentManager.Frontend.Controllers
{
    public class PlayerController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public PlayerController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var token = Request.Cookies["AccessToken"];
            if (token is null)
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient("TournamentManagerApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var profileResponse = await client.GetAsync("api/Player/profile");

            if (profileResponse.IsSuccessStatusCode)
            {
                var player = await profileResponse.Content.ReadFromJsonAsync<PlayerResponse>();
                return View("Profile", player);
            }

            var teamsResponse = await client.GetAsync("api/Team");
            var teams = await teamsResponse.Content.ReadFromJsonAsync<List<TeamResponse>>();
            ViewBag.Teams = teams ?? new List<TeamResponse>();
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Profile([FromForm] CreatePlayerRequest createPlayerRequest)
        {
            var token = Request.Cookies["AccessToken"];
            if (token is null)
                return RedirectToAction("Login", "Auth");

            var client = _httpClientFactory.CreateClient("TournamentManagerApi");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PostAsJsonAsync("api/Player/profile", createPlayerRequest);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                ViewData["Error"] = string.IsNullOrWhiteSpace(error) ? "Failed to create profile." : error;

                var teamsResponse = await client.GetAsync("api/Team");
                ViewBag.Teams = await teamsResponse.Content.ReadFromJsonAsync<List<TeamResponse>>() ?? new List<TeamResponse>();

                return View("Create", createPlayerRequest);
            }

            return RedirectToAction("Profile");
        }
    }
}
