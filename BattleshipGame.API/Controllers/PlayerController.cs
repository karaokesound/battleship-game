using BattleshipGame.API.Models;
using BattleshipGame.API.Stores;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerDto> _logger;

        public PlayerController(ILogger<PlayerDto> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlayerDto>> GetPlayers()
        {
            return Ok(PlayersDataStore.Current.Players);
        }

        [HttpGet("{playerid}", Name = "GetPlayer")]
        public ActionResult<PlayerDto> GetPlayer(int playerId)
        {
            PlayerDto selectedPlayer = PlayersDataStore.Current.Players
                .FirstOrDefault(i => i.Id == playerId);

            if (selectedPlayer == null)
            {
                _logger.LogWarning($"Player with id {playerId} doesn't found");
                return NotFound();
            }

            return Ok(selectedPlayer);
        }

        [HttpPost]
        public ActionResult<PlayerDto> AddPlayer(int id, PlayerForCreationDto playerForCreation)
        {
            var maxExistingId = PlayersDataStore.Current.Players
                .Max(i => i.Id);

            var newPlayer = new PlayerDto()
            {
                Id = ++maxExistingId,
                Name = playerForCreation.Name,
                City = playerForCreation.City,
            };

            PlayersDataStore.Current.Players.Add(newPlayer);

            return CreatedAtRoute("GetPlayer",
                new
                {
                    //playerId - refers to route template in GetPlayer method
                    playerId = newPlayer.Id
                },
                newPlayer
                );
        }

        [HttpPut("{playerid}")]
        public ActionResult UpdatePlayer(int playerId, PlayerForUpdateDto playerForUpdate)
        {
            var selectedPlayer = PlayersDataStore.Current.Players
                .FirstOrDefault(i => i.Id == playerId);

            if (selectedPlayer == null) return NotFound("Sorry! There is no user with this id. Try again.");

            selectedPlayer.Name = playerForUpdate.Name;
            selectedPlayer.City = playerForUpdate.City;

            return NoContent();
        }

        [HttpPatch("{playerId}")]
        public ActionResult PartiallyUpdatePlayer(int playerId, JsonPatchDocument<PlayerForUpdateDto> patchDocument)
        {
            var selectedPlayer = PlayersDataStore.Current.Players
                .FirstOrDefault(i => i.Id == playerId);

            var playerToPatch = new PlayerForUpdateDto()
            {
                Name = selectedPlayer.Name,
                City = selectedPlayer.City
            };

            patchDocument.ApplyTo(playerToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TryValidateModel(playerToPatch)) return BadRequest();

            selectedPlayer.Name = playerToPatch.Name;
            selectedPlayer.City = playerToPatch.City;

            return NoContent();
        }

        [HttpDelete("{playerid}")]
        public ActionResult DeletePlayer(int playerId)
        {
            var selectedPlayer = PlayersDataStore.Current.Players
                .FirstOrDefault(i => i.Id == playerId);

            if (selectedPlayer == null) return NotFound();

            PlayersDataStore.Current.Players.Remove(selectedPlayer);

            return Ok("Player successfully deleted");
        }
    }
}
