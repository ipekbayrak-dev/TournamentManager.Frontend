using TournamentManager.Frontend.Models.Enums;

namespace TournamentManager.Frontend.Models.Match
{
    public class UpdateMatchRequest
    {
        public Guid Id { get; set; }
        public int TeamRadiantScore { get; set; }
        public int TeamDireScore { get; set; }
        public MatchStatus Status { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid? WinnerTeamId { get; set; }
    }
}
