using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models;

namespace TournamentManager.Frontend.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        ViewData["ApiUrl"] = _configuration["ApiSettings:BaseUrl"];
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
