using TournamentManager.Frontend.Models.Enums;

namespace TournamentManager.Frontend.Models.Player
{
    public class PlayerResponse
    {
        public Guid Id { get; set; }
        public required string Handle { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required string CountryCode { get; set; }
        public PlayerStatus Status { get; set; }
        public DotaPosition Position { get; set; }
        public bool IsCaptain { get; set; }
        public string? SteamId { get; set; }
        public Guid TeamId { get; set; }
    }
}
