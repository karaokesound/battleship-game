using System.ComponentModel.DataAnnotations;

namespace BattleshipGame.API.Models
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
