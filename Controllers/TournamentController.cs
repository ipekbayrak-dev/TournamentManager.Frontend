using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Player;
using TournamentManager.Frontend.Models.Team;
using TournamentManager.Frontend.Models.Tournament;
using TournamentManager.Frontend.Models.TournamentEntry;

namespace TournamentManager.Frontend.Controllers
{
    public class TournamentController : BaseController
    {
        public TournamentController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null)
            {
                return redirect!;
            }

            var response = await client.GetAsync("api/Tournament");

            var check = CheckUnauthorized(response);
            if (check is not null)
            {
                return check;
            }

            var tournaments = await response.Content.ReadFromJsonAsync<List<TournamentResponse>>();
            return View(tournaments);
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var tournamentTask = client.GetAsync($"api/Tournament/slug/{id}");
            var teamsTask = client.GetAsync("api/Team");
            var playerTask = client.GetAsync("api/Player/profile");

            await Task.WhenAll(tournamentTask, teamsTask, playerTask);

            var check = CheckUnauthorized(tournamentTask.Result);

            if (check is not null)
            {
                return check;
            }

            if (tournamentTask.Result.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            var tournament = await tournamentTask.Result.Content.ReadFromJsonAsync<TournamentResponse>();

            if (tournament is null)
            {
                return NotFound();
            }

            var teams = await teamsTask.Result.Content.ReadFromJsonAsync<List<TeamResponse>>() ?? new();
            ViewBag.Teams = teams.ToDictionary(t => t.Id);

            PlayerResponse? playerProfile = null;

            if (playerTask.Result.IsSuccessStatusCode)
                playerProfile = await playerTask.Result.Content.ReadFromJsonAsync<PlayerResponse>();

            ViewBag.PlayerProfile = playerProfile;

            return View(tournament);
        }
        [HttpPost]
        public async Task<IActionResult> WithdrawTeam([FromForm] Guid entryId, [FromForm] string slug)
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null) return redirect!;

            var response = await client.DeleteAsync($"api/TournamentEntry/{entryId}");

            var check = CheckUnauthorized(response);
            if (check is not null) return check;

            if (!response.IsSuccessStatusCode)
                TempData["Error"] = await response.Content.ReadAsStringAsync();

            return RedirectToAction("Detail", new { id = slug });
        }

        [HttpPost]
        public async Task<IActionResult> RegisterTeam([FromForm] Guid tournamentId, [FromForm] Guid teamId, [FromForm] string slug, [FromForm] string tournamentName)
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null) return redirect!;

            var response = await client.PostAsJsonAsync("api/TournamentEntry", new CreateTournamentEntryRequest { TournamentId = tournamentId, TeamId = teamId });

            var check = CheckUnauthorized(response);
            if (check is not null) return check;

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = await response.Content.ReadAsStringAsync();
                return RedirectToAction("Detail", new { id = slug });
            }

            var entry = await response.Content.ReadFromJsonAsync<TournamentEntryResponse>();
            return RedirectToAction("Checkout", "Payment", new
            {
                entryId = entry!.Id,
                slug = slug,
                tournamentName = tournamentName
            });
        }
    }
}
