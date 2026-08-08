using TournamentManager.Frontend.Models.Enums;

namespace TournamentManager.Frontend.Models.Tournament
{
    public class TournamentResponse
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
        public string? Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public TournamentStatus Status { get; set; }
    }
}