using AutoMapper;
using BattleshipGame.API.Models;
using BattleshipGame.API.Services;
using BattleshipGame.API.Stores;
using BattleshipGame.Data.Entities;
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

        private readonly IMapper _mapper;

        public PlayerController(ILogger<PlayerDto> logger, IPlayersRepository playersRepository, IMapper mapper)
        {
            _logger = logger;
            _playersRepository = playersRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerDto>>> GetPlayers()
        {
            var dbPlayers = await _playersRepository.GetPlayersAsync();

            List<PlayerDto> players = new List<PlayerDto>();

            return Ok(_mapper.Map<IEnumerable<PlayerDto>>(dbPlayers));
        }

        [HttpGet("{playerid}", Name = "GetPlayer")]
        public async Task<ActionResult<PlayerDto>> GetPlayer(int playerId)
        {
            var selectedPlayer = await _playersRepository.GetPlayerAsync(playerId);

            if (selectedPlayer == null)
            {
                _logger.LogWarning($"Player with id {playerId} doesn't found");
                return NotFound();
            }

            return Ok(_mapper.Map<PlayerDto>(selectedPlayer));
        }

        [HttpPost]
        public async Task<ActionResult<PlayerDto>> CreatePlayer(PlayerForCreationDto playerForCreation)
        {
            var newPlayer = _mapper.Map<Player>(playerForCreation);

            await _playersRepository.CreatePlayerAsync(newPlayer);

            await _playersRepository.SaveChangesAsync();

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
        public async Task<ActionResult> UpdatePlayer(int playerId, PlayerForUpdateDto playerForUpdate)
        {
            var dbPlayer = await _playersRepository.GetPlayerAsync(playerId);

            if (dbPlayer == null) return NotFound("Sorry! There is no user with this id. Try again.");

            _mapper.Map(playerForUpdate, dbPlayer);

            await _playersRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{playerId}")]
        public async Task<ActionResult> PartiallyUpdatePlayer(int playerId, JsonPatchDocument<PlayerForUpdateDto> patchDocument)
        {
            if (_playersRepository.GetPlayerAsync(playerId) == null) return NotFound();

            var selectedPlayer = await _playersRepository.GetPlayerAsync(playerId);

            var playerToPatch = _mapper.Map<PlayerForUpdateDto>(selectedPlayer);

            // This method applies provided changes and properties with no changes stays as their are.
            // Without this patching, properties with no changes would be null.
            patchDocument.ApplyTo(playerToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TryValidateModel(playerToPatch)) return BadRequest();

            _mapper.Map(playerToPatch, selectedPlayer);

            await _playersRepository.SaveChangesAsync();
            
            return NoContent();
        }

        [HttpDelete("{playerid}")]
        public async Task<ActionResult> DeletePlayer(int playerId)
        {
            var selectedPlayer = await _playersRepository.GetPlayerAsync(playerId);

            if (selectedPlayer == null) return NotFound();

            _playersRepository.DeletePlayer(selectedPlayer);
            await _playersRepository.SaveChangesAsync();

            return Ok("Player successfully deleted");
        }
    }
}
