using BattleshipGame.API.Services;
using BattleshipGame.API.Services.Controllers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using static BattleshipGame.API.Services.Controllers.GamePlayService;

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
        
        [SwaggerResponse(StatusCodes.Status200OK, "Shot was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid operation.")]
        [HttpPatch("shoot/{playerName}")]
        public async Task<ActionResult> ShootByPlayer(
            [SwaggerParameter(Description = "Name of the player who shoots [your nickname]")] string playerName,
            [SwaggerParameter(Description = "Coordinates in the format (x,y): 0,1 2,1")] string coordinates)
        {
            var players = await _service.GetPlayers();

            var result = await _service.PlayersAndCoordsValidation(playerName, coordinates);

            if (result != "") return BadRequest(result);

            result = await _service.FlagCheck(0);

            if (result != "") return BadRequest(result);

            // Checking if inserted coordinates were used in the previous rounds

            List<string> fieldsAlreadyHit = await _service.GetHitFields(coordinates);
            List<string> allHitFields = await _service.GetAllHitFields(players[1].Name);

            if (fieldsAlreadyHit.Count > 0)
            {
                var jsonResult = new JsonResult(fieldsAlreadyHit);
                var jsonResult2 = new JsonResult(allHitFields);

                return BadRequest(new { Message = "This field(s) was hit before! List of all hit fields is displayed below." +
                    " Try with another coordinate!",
                    Data = jsonResult.Value, jsonResult2.Value });
            }

            // Taking inserted coordinates and updating opponent player's gameboard. Additionally the method sets IsHitted property and
            // count sunken ships by the player. If player hit any field with a ship on it, in "response" we will receive coordinates
            // of all such fields

            CombinedResponseData response = await _service.UpdatePlayerFields(playerName, coordinates);

            if (response.Message != null) return Ok(response);
                
            return Ok(_message.ShotMissed(players, playerName));
        }

        [HttpPatch("shoot/opponent")]
        public async Task<ActionResult> RandomShootByOpponent()
        {
            var players = await _service.GetPlayers();

            string result = await _service.FlagCheck(1);

            if (result != "") return BadRequest(result);

            // Setting random coordinates to shoot by computer and then updating player's gameboard. Additionally
            // the method sets IsHitted property and count sunken ships by computer. If computer hit any field with
            // a ship on it, in "response" we will receive coordinates of all such fields

            CombinedResponseData response = await _service.UpdatePlayerFields(players[1].Name, "");

            if (response.Message != null) return Ok(response);

            var refreshedGameBoard = await _service.RefreshGameBoard();
            var message = _message.ShotMissed(players, players[1].Name);

            return Ok(new { Message = message, Data = refreshedGameBoard });
        }
    }
}
