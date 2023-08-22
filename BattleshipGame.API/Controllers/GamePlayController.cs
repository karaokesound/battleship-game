using BattleshipGame.API.Services;
using BattleshipGame.API.Services.Controllers;
using BattleshipGame.API.Services.Repositories;
using BattleshipGame.Data.Entities;
using BattleshipGame.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.Numerics;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GamePlayController : ControllerBase
    {
        private readonly IValidationService _validation;

        private readonly iFieldRepository _fieldRepository;

        private readonly IPlayersRepository _playersRepository;

        private readonly IMessageService _message;

        private readonly IGameRepository _gameRepository;

        private readonly IGamePlayService _service;

        public GamePlayController(IValidationService validation,
            iFieldRepository fieldRepository,
            IPlayersRepository playersRepository,
            IMessageService message,
            IGameRepository gameRepository,
            IGamePlayService service)
        {
            _validation = validation;
            _fieldRepository = fieldRepository;
            _playersRepository = playersRepository;
            _message = message;
            _gameRepository = gameRepository;
            _service = service;
        }
        
        [SwaggerOperation(Summary = "Shoots at target coordinates.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Shot was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid coordinates format.")]
        [HttpPatch("shoot/{playerName}")]
        public async Task<ActionResult> ShootAtCoordinates(
            [SwaggerParameter(Description = "Name of the player who shoots [your nickname]")] string playerName,
            [SwaggerParameter(Description = "Coordinates in the format (x,y): 0,1 2,1")] string coordinates)
        {
            var result = await _service.ValidatePlayersAndCoordinates(playerName, coordinates);

            if (result != "") return BadRequest(result);

            List<string> hittedShipsCoords = await _service.UpdatePlayerFields(playerName, coordinates);

            if (hittedShipsCoords.Count > 0)
            {
                var message = _message.ShotSuccess(hittedShipsCoords.Count);
                var jsonResult = new JsonResult(hittedShipsCoords);

                return Ok(new { Message = message, Data = jsonResult.Value });
            }

            return Ok(_message.ShotMissed());
        }

        [HttpPatch("shoot/opponent")]
        public async Task<ActionResult> RandomShootByOpponent()
        {
            var players = await _service.GetPlayers();
            string result = await _service.ValidateOpponentTurn();

            if (result != "") return BadRequest(result);

            var hittedShips = await _service.SetRandomShotByOpponent();

            if (hittedShips.Count > 0)
            {
                var message = $"{players[1].Name} hit {hittedShips.Count} of your field(s). See details below.";
                var jsonResult = new JsonResult(hittedShips);

                return Ok(new { Message = message, Data = jsonResult.Value });
            }

            return Ok("DUPA");

            //await DisplayGameBoard(player.Name);

            //return Ok(await DisplayGameBoard(player.Name));
        }
    }
}
