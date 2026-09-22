namespace TournamentManager.Frontend.Models.Payment
{
    public class CreateCheckoutSessionRequest
    {
        public Guid TournamentEntryId { get; set; }
        public string TournamentName { get; set; } = string.Empty;
        public string TournamentSlug { get; set; } = string.Empty;
    }
}
