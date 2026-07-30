using System.ComponentModel.DataAnnotations;

namespace TournamentManager.Frontend.Models.Auth
{
    public class SignInRequest
    {
        public string Email { get; set; } = string.Empty;

        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;
    }
}