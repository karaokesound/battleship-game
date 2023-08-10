using System.ComponentModel.DataAnnotations;

namespace BattleshipGame.API.Models.Player
{
    public class PlayerForUpdateDto
    {
        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(20)]
        public string City { get; set; }
    }
}
