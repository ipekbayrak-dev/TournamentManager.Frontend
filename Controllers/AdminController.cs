using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using TournamentManager.Frontend.Models.Player;
using TournamentManager.Frontend.Models.Team;
using TournamentManager.Frontend.Models.Tournament;

namespace TournamentManager.Frontend.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var check = CheckAdminAccess();
            if (check is not null) context.Result = check;
            base.OnActionExecuting(context);
        }

        [HttpGet]
        public async Task<IActionResult> Index() => View();
        [HttpGet]
        public async Task<IActionResult> Tournaments()
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
        public async Task<IActionResult> CreateTournament() => View();
        [HttpPost]
        public async Task<IActionResult> CreateTournament([FromForm] CreateTournamentRequest createTournamentRequest)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.PostAsJsonAsync("api/Tournament", createTournamentRequest);

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = await response.Content.ReadAsStringAsync();
                return View(createTournamentRequest);
            }
            return RedirectToAction("Tournaments");
        }
        [HttpGet]
        public async Task<IActionResult> EditTournament(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.GetAsync($"api/Tournament/{id}");

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            var tournament = await response.Content.ReadFromJsonAsync<TournamentResponse>();

            var model = new UpdateTournamentRequest
            {
                Id = tournament!.Id,
                Name = tournament.Name,
                Description = tournament.Description,
                Location = tournament.Location,
                StartDate = tournament.StartDate,
                EndDate = tournament.EndDate,
                Status = tournament.Status
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditTournament([FromForm] UpdateTournamentRequest updateTournamentRequest)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.PutAsJsonAsync($"api/Tournament/{updateTournamentRequest.Id}", updateTournamentRequest);

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }


            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = await response.Content.ReadAsStringAsync();
                return View(updateTournamentRequest);
            }

            return RedirectToAction("Tournaments");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTournament(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.DeleteAsync($"api/Tournament/{id}");

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            return RedirectToAction("Tournaments");
        }
        [HttpGet]
        public async Task<IActionResult> PendingPlayers()
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.GetAsync("api/Player/pending");

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            var player = await response.Content.ReadFromJsonAsync<List<PlayerResponse>>();

            return View(player);

        }
        [HttpPost]
        public async Task<IActionResult> ApprovePlayer(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null)
            {
                return redirect!;
            }

            var response = await client.PutAsJsonAsync($"api/Player/{id}/status", 2);

            var check = CheckUnauthorized(response);
            if (check is not null)
            {
                return check;
            }

            return RedirectToAction("PendingPlayers");
        }

        [HttpPost]
        public async Task<IActionResult> RejectPlayer(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null)
            {
                return redirect!;
            }

            var response = await client.PutAsJsonAsync($"api/Player/{id}/status", 3);

            var check = CheckUnauthorized(response);
            if (check is not null)
            {
                return check;
            }

            return RedirectToAction("PendingPlayers");
        }
        [HttpGet]
        public async Task<IActionResult> Teams()
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.GetAsync("api/Team");

            var check = CheckUnauthorized(response);
            if (check is not null)
            {
                return check;
            }

            var teams = await response.Content.ReadFromJsonAsync<List<TeamResponse>>();

            return View(teams);
        }
        [HttpGet]
        public async Task<IActionResult> CreateTeam() => View();
        [HttpPost]
        public async Task<IActionResult> CreateTeam([FromForm] CreateTeamRequest createTeamRequest)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.PostAsJsonAsync("api/Team", createTeamRequest);

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = await response.Content.ReadAsStringAsync();
                return View(createTeamRequest);
            }
            return RedirectToAction("Teams");
        }
        [HttpGet]
        public async Task<IActionResult> EditTeam(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.GetAsync($"api/Team/{id}");

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            var team = await response.Content.ReadFromJsonAsync<TeamResponse>();

            var model = new UpdateTeamRequest
            {
                Id = id,
                Handle = team!.Handle,
                Logo = team.Logo,
                Name = team.Name,
                Region = team.Region
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditTeam([FromForm] UpdateTeamRequest updateTeamRequest)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.PutAsJsonAsync($"api/Team/{updateTeamRequest.Id}", updateTeamRequest);

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }


            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = await response.Content.ReadAsStringAsync();
                return View(updateTeamRequest);
            }

            return RedirectToAction("Teams");
        }
        [HttpPost]
        public async Task<IActionResult> DeleteTeam(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.DeleteAsync($"api/Team/{id}");

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            return RedirectToAction("Teams");
        }
        [HttpGet]
        public async Task<IActionResult> Players()
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.GetAsync("api/Player");

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            var players = await response.Content.ReadFromJsonAsync<List<PlayerResponse>>();

            return View(players);
        }
        [HttpGet]
        public async Task<IActionResult> EditPlayer(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.GetAsync($"api/Player/{id}");

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            var team = await response.Content.ReadFromJsonAsync<PlayerResponse>();

            var model = new UpdatePlayerRequest
            {
                Id = id,
                TeamId = team!.TeamId,
                CountryCode = team.CountryCode,
                FirstName = team.FirstName,
                Handle = team.Handle,
                LastName = team.LastName,
                Position = team.Position,
                IsCaptain = team.IsCaptain,
                SteamId = team.SteamId
            };

            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> EditPlayer([FromForm] UpdatePlayerRequest updatePlayerRequest)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.PutAsJsonAsync($"api/Player/{updatePlayerRequest.Id}", updatePlayerRequest);

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }


            if (!response.IsSuccessStatusCode)
            {
                ViewData["Error"] = await response.Content.ReadAsStringAsync();
                return View(updatePlayerRequest);
            }

            return RedirectToAction("Players");
        }
        [HttpPost]
        public async Task<IActionResult> DeletePlayer(Guid id)
        {
            var client = CreateAuthorizedClient(out var redirect);

            if (client is null)
            {
                return redirect!;
            }

            var response = await client.DeleteAsync($"api/Player/{id}");

            var check = CheckUnauthorized(response);

            if (check is not null)
            {
                return check;
            }

            return RedirectToAction("Players");
        }
    }
}