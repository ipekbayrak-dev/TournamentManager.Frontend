using Microsoft.AspNetCore.Mvc;
using TournamentManager.Frontend.Models.Payment;

namespace TournamentManager.Frontend.Controllers
{
    public class PaymentController : BaseController
    {
        public PaymentController(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        [HttpGet]
        public async Task<IActionResult> Checkout(Guid entryId, string slug, string tournamentName)
        {
            var client = CreateAuthorizedClient(out var redirect);
            if (client is null) return redirect!;

            var response = await client.PostAsJsonAsync("api/Payment/create-checkout-session", new CreateCheckoutSessionRequest
            {
                TournamentEntryId = entryId,
                TournamentName = tournamentName,
                TournamentSlug = slug
            });

            var check = CheckUnauthorized(response);
            if (check is not null) return check;

            if (!response.IsSuccessStatusCode)
            {
                TempData["Error"] = "Could not initiate payment. Please try again.";
                return RedirectToAction("Detail", "Tournament", new { id = slug });
            }

            var result = await response.Content.ReadFromJsonAsync<CreateCheckoutSessionResponse>();
            return Redirect(result!.SessionUrl);
        }

        [HttpGet]
        public IActionResult Success(string slug)
        {
            ViewBag.Slug = slug;
            return View();
        }
    }
}
