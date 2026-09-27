namespace TournamentManager.Frontend.Models.TournamentEntry
{
    public class PendingEntryViewModel
    {
        public Guid Id { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public string TournamentName { get; set; } = string.Empty;
        public int? Seed { get; set; }
        public DateTime RegisteredAt { get; set; }
    }
}
