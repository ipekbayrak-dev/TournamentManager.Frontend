using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Player;
using TournamentManager.Frontend.Models.Team;

namespace TournamentManager.Frontend.Controllers
{
    public class TeamController : BaseController
    {
        public TeamController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        [HttpGet]
        public async Task<IActionResult> Detail(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null) return redirect!;

            var teamTask = client.GetAsync($"api/Team/{id}");
            var playersTask = client.GetAsync($"api/Player/team/{id}");

            await Task.WhenAll(teamTask, playersTask);

            var check = CheckUnauthorized(teamTask.Result);
            if (check is not null) return check;

            if (teamTask.Result.StatusCode == System.Net.HttpStatusCode.NotFound)
                return NotFound();

            var team = await teamTask.Result.Content.ReadFromJsonAsync<TeamResponse>();
            if (team is null) return NotFound();

            var players = playersTask.Result.IsSuccessStatusCode
                ? await playersTask.Result.Content.ReadFromJsonAsync<List<PlayerResponse>>() ?? new()
                : new List<PlayerResponse>();

            ViewBag.Players = players;
            return View(team);
        }
    }
}
