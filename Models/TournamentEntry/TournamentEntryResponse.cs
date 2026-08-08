using TournamentManager.Frontend.Models.Enums;
namespace TournamentManager.Frontend.Models.TournamentEntry
{
    public class TournamentEntryResponse
    {
        public Guid Id { get; set; }
        public Guid TournamentId { get; set; }
        public Guid TeamId { get; set; }
        public DateTime RegisteredAt { get; set; }
        public int? Seed { get; set; }
        public EntryStatus Status { get; set; }
    }
}