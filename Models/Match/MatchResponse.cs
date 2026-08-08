
using TournamentManager.Frontend.Models.Enums;

namespace TournamentManager.Frontend.Models.Match
{
    public class MatchResponse
    {
        public Guid Id { get; set; }
        public BracketType BracketType { get; set; }
        public int RoundNumber { get; set; }
        public Guid TournamentId { get; set; }
        public Guid? TeamRadiantId { get; set; }
        public Guid? TeamDireId { get; set; }
        public int TeamRadiantScore { get; set; }
        public int TeamDireScore { get; set; }
        public MatchStatus Status { get; set; }
        public DateTime ScheduledAt { get; set; }
        public DateTime? CompletedAt { get; set; }
        public Guid? WinnerTeamId { get; set; }
        public Guid? WinnerAdvancesToMatchId { get; set; }
        public Guid? LoserAdvancesToMatchId { get; set; }
    }
}