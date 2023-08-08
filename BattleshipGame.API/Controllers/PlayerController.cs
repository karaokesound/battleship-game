using BattleshipGame.API.Models;
using BattleshipGame.API.Stores;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<PlayerDto>> GetPlayers()
        {
            return Ok(PlayersDataStore.Current.Players);
        }

        [HttpGet("{id}")]
        public ActionResult<PlayerDto> GetPlayer(int id)
        {
            PlayerDto player = PlayersDataStore.Current.Players
                .FirstOrDefault(i => i.Id == id);

            if (player == null) return NotFound();

            return Ok(player);
        }
    }
}
