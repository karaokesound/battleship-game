using BattleshipGame.API.Models;
using BattleshipGame.API.Services;
using BattleshipGame.API.Stores;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Services.Common;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/players")]
    public class PlayerController : ControllerBase
    {
        private readonly ILogger<PlayerDto> _logger;

        private readonly IPlayersRepository _playersRepository;

        public PlayerController(ILogger<PlayerDto> logger, IPlayersRepository playersRepository)
        {
            _logger = logger;
            _playersRepository = playersRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
        {
            var dbPlayers = await _playersRepository.GetPlayers();

            List<PlayerDto> players = new List<PlayerDto>();

            foreach (var player in dbPlayers)
            {
                var playerDto = new PlayerDto()
                {
                    Id = player.Id,
                    Name = player.Name,
                    City = player.City,
                };

                players.Add(playerDto);
            }

            return Ok(players);
        }

        [HttpGet("{playerid}", Name = "GetPlayer")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int playerId)
        {
            var selectedPlayer = await _playersRepository.GetPlayer(playerId);

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
