using BattleshipGame.API.Services;
using BattleshipGame.API.Services.Controllers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GamePlayController : ControllerBase
    {
        private readonly IMessageService _message;
        private readonly IGamePlayService _service;

        public GamePlayController(IMessageService message, IGamePlayService service)
        {
            _message = message;
            _service = service;
        }
        
        [SwaggerOperation(Summary = "Shoots at target coordinates.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Shot was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid coordinates format.")]
        [HttpPatch("shoot/{playerName}")]
        public async Task<ActionResult> ShootByPlayer(
            [SwaggerParameter(Description = "Name of the player who shoots [your nickname]")] string playerName,
            [SwaggerParameter(Description = "Coordinates in the format (x,y): 0,1 2,1")] string coordinates)
        {
            var players = await _service.GetPlayers();
            var result = await _service.PlayersAndCoordsNullCheck(playerName, coordinates);

            if (result != "") return BadRequest(result);

            result = await _service.FlagCheck(0);

            if (result != "") return BadRequest(result);

            List<string> fieldsAlreadyHit = await _service.CoordinatesCheck(coordinates);
            List<string> allHitFields = await _service.GetAllHitFields(players[1].Name);

            if (fieldsAlreadyHit.Count > 0)
            {
                var jsonResult = new JsonResult(fieldsAlreadyHit);
                var jsonResult2 = new JsonResult(allHitFields);

                return BadRequest(new { Message = "This field was already hit! Hit field which wasn't used before.",
                    Data = jsonResult.Value, jsonResult2.Value });
            }

            // Updating opponent player's fields and returning hit fields that had a ship on them

            List<string> hitShipsCoords = await _service.UpdatePlayerFields(playerName, coordinates);

            if (hitShipsCoords.Count == 1)
            {
                var message = _message.ShotSuccess(hitShipsCoords.Count, hitShipsCoords);
                return Ok(message);
            }
            else if (hitShipsCoords.Count > 1)
            {
                var jsonResult = new JsonResult(hitShipsCoords);

                return Ok(new { Message = _message.ShotSuccess(hitShipsCoords.Count, hitShipsCoords), 
                    Data = jsonResult.Value });
            }
                
            return Ok(_message.ShotMissed());
        }

        [HttpPatch("shoot/opponent")]
        public async Task<ActionResult> RandomShootByOpponent()
        {
            var players = await _service.GetPlayers();

            string result = await _service.FlagCheck(1);

            if (result != "") return BadRequest(result);

            // Setting random coordinates to shoot (computer move) and then updating player's (our) fields and
            // returning hit fields that had a ship on them

            var hittedShipsCoords = await _service.SetRandomShootAndUpdateFields();

            if (hittedShipsCoords.Count > 0)
            {
                var message = $"{players[1].Name} hit {hittedShipsCoords.Count} of your field(s). See details below.";
                var jsonResult = new JsonResult(hittedShipsCoords);

                return Ok(new { Message = message, Data = jsonResult.Value });
            }

            var refreshedGameBoard = await _service.RefreshGameBoard();

            return Ok(new { Message = "Opponent missed", Data = refreshedGameBoard});
        }
    }
}
