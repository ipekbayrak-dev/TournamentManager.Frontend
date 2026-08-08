using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Tournament;
using TournamentManager.Frontend.Models.Auth;
using System.Net.Http.Headers;

namespace TournamentManager.Frontend.Controllers
{
    public class TournamentController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public TournamentController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = Request.Cookies["AccessToken"];

            if (token is null)
            {
                return RedirectToAction("Login","Auth");
            }

            var client = _httpClientFactory.CreateClient("TournamentManagerApi");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            
            var response = await client.GetAsync("api/Tournament");

            var tournaments = await response.Content.ReadFromJsonAsync<List<TournamentResponse>>();
            
            return View(tournaments);
        }
    }
}