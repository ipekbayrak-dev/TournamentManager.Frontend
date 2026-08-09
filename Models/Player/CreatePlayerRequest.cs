using System.ComponentModel.DataAnnotations;
using TournamentManager.Frontend.Models.Enums;

namespace TournamentManager.Frontend.Models.Player
{
    public class CreatePlayerRequest
    {
        [Required(ErrorMessage = "Handle is required.")]
        public string Handle { get; set; } = string.Empty;

        [Required(ErrorMessage = "First name is required.")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required.")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country code is required.")]
        public string CountryCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required.")]
        public DotaPosition Position { get; set; }

        public bool IsCaptain { get; set; }

        public string? SteamId { get; set; }

        [Required(ErrorMessage = "Team is required.")]
        public Guid TeamId { get; set; }
    }
}