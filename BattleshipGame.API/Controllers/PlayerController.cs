using BattleshipGame.API.Models;
using BattleshipGame.API.Stores;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Services.WebApi.Patch;

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

        [HttpGet("{playerid}", Name = "GetPlayer")]
        public ActionResult<PlayerDto> GetPlayer(int playerId)
        {
            PlayerDto player = PlayersDataStore.Current.Players
                .FirstOrDefault(i => i.Id == playerId);

            if (player == null) return NotFound();

            return Ok(player);
        }

        [HttpPost]
        public ActionResult<PlayerDto> AddPlayer(int id, PlayerForCreationDto playerForCreation)
        {
            var maxExistingId = PlayersDataStore.Current.Players
                .Max(i => i.Id);

            var newPlayer = new PlayerDto()
            {
                Id = maxExistingId + 1,
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
            var playerToUpdate = PlayersDataStore.Current.Players
                .FirstOrDefault(i => i.Id == playerId);

            if (playerToUpdate == null) return NotFound("Sorry! There is no user with this id. Try again.");

            playerToUpdate.Name = playerForUpdate.Name;
            playerToUpdate.City = playerForUpdate.City;

            return NoContent();
        }

        [HttpPatch("{playerId}")]
        public ActionResult PartiallyUpdatePlayer(int playerId, JsonPatchDocument<PlayerForUpdateDto> patchDocument)
        {
            var playerFromStore = PlayersDataStore.Current.Players
                .FirstOrDefault(i => i.Id == playerId);

            var playerToPatch = new PlayerForUpdateDto()
            {
                Name = playerFromStore.Name,
                City = playerFromStore.City
            };

            patchDocument.ApplyTo(playerToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            playerFromStore.Name = playerToPatch.Name;
            playerFromStore.City = playerToPatch.City;

            return NoContent();
        }
    }
}
