using System.ComponentModel.DataAnnotations;

namespace TournamentManager.Frontend.Models.Tournament
{
    public class CreateTournamentRequest
    {
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
    }
}
