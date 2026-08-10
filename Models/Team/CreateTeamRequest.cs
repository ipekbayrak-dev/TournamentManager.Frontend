
using TournamentManager.Frontend.Models.Enums;

namespace TournamentManager.Frontend.Models.Team
{
    public class CreateTeamRequest
    {
        public required string Name { get; set; }
        public required string Handle { get; set; }
        public required string Logo { get; set; }
        public DotaRegion Region { get; set; }
    }
}