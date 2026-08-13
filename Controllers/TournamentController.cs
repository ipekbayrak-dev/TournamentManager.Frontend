using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Team;
using TournamentManager.Frontend.Models.Tournament;

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
        public async Task<IActionResult> Detail(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var tournamentTask = client.GetAsync($"api/Tournament/{id}");
            var teamsTask = client.GetAsync("api/Team");

            await Task.WhenAll(tournamentTask, teamsTask);

            var check = CheckUnauthorized(tournamentTask.Result);
            if (check is not null)
            {
                return check;
            }

            var tournament = await tournamentTask.Result.Content.ReadFromJsonAsync<TournamentResponse>();
            
            if (tournament is null)
            {
                return NotFound();
            }

            var teams = await teamsTask.Result.Content.ReadFromJsonAsync<List<TeamResponse>>() ?? new();
            ViewBag.Teams = teams.ToDictionary(t => t.Id);

            return View(tournament);
        }
    }
}
