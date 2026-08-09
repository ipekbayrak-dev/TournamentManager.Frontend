using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Tournament;

namespace TournamentManager.Frontend.Controllers
{
    public class AdminController : BaseController
    {
        public AdminController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }
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
    }
}