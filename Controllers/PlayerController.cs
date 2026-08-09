using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Player;
using TournamentManager.Frontend.Models.Team;

namespace TournamentManager.Frontend.Controllers
{
    public class PlayerController : BaseController
    {
        public PlayerController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null)
            {
                return redirect!;
            }

            var profileResponse = await client.GetAsync("api/Player/profile");

            var check = CheckUnauthorized(profileResponse);
            if (check is not null)
            {
                return check;
            }

            if (profileResponse.IsSuccessStatusCode)
            {
                var player = await profileResponse.Content.ReadFromJsonAsync<PlayerResponse>();
                return View("Profile", player);
            }

            var teamsResponse = await client.GetAsync("api/Team");
            var teamsCheck = CheckUnauthorized(teamsResponse);
            if (teamsCheck is not null)
            {
                return teamsCheck;
            }

            var teams = await teamsResponse.Content.ReadFromJsonAsync<List<TeamResponse>>();
            ViewBag.Teams = teams ?? new List<TeamResponse>();
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> Profile([FromForm] CreatePlayerRequest createPlayerRequest)
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null)
            {
                return redirect!;
            }

            var response = await client.PostAsJsonAsync("api/Player/profile", createPlayerRequest);

            var check = CheckUnauthorized(response);
            if (check is not null)
            {
                return check;
            }

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
