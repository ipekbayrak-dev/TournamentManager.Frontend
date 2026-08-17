namespace TournamentManager.Frontend.Models.TournamentEntry
{
    public class CreateTournamentEntryRequest
    {
        public Guid TournamentId { get; set; }
        public Guid TeamId { get; set; }
    }
}