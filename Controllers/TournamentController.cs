using Microsoft.AspNetCore.Mvc;
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
    }
}
