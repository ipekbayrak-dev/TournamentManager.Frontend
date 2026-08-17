using System.ComponentModel.DataAnnotations;
using TournamentManager.Frontend.Models.Enums;

namespace TournamentManager.Frontend.Models.Tournament
{
    public class UpdateTournamentRequest
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "Tournament name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug is required.")]
        public string Slug { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Location { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        public TournamentStatus Status { get; set; }
    }
}
