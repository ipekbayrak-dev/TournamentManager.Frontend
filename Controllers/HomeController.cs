using System.Diagnostics;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models;
using TournamentManager.Frontend.Models.Enums;
using TournamentManager.Frontend.Models.Tournament;

namespace TournamentManager.Frontend.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IConfiguration configuration, IHttpClientFactory httpClientFactory)
    {
        _configuration = configuration;
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["ApiUrl"] = _configuration["ApiSettings:BaseUrl"];

        try
        {
            var client = _httpClientFactory.CreateClient("TournamentManagerApi");
            var token = HttpContext.Request.Cookies["AccessToken"];
            if (token is not null)
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("api/Tournament");
            if (response.IsSuccessStatusCode)
            {
                var all = await response.Content.ReadFromJsonAsync<List<TournamentResponse>>() ?? new();
                ViewBag.OngoingTournaments = all.Where(t => t.Status == TournamentStatus.Ongoing).ToList();
            }
        }
        catch { /* home page stays functional even if API is down */ }

        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Compete()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
