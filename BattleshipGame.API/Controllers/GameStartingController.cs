using BattleshipGame.API.Services;
using BattleshipGame.API.Services.Controllers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameStartingController : ControllerBase
    {
        private readonly IMessageService _message;
        private readonly IGameStartingService _service;

        public GameStartingController(IMessageService message,
            IGameStartingService service)
        {
            _message = message;
            _service = service;
        }

        [SwaggerOperation(Summary = "Start the game.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid key or player name.")]
        [HttpPost("start/{key}/{playerName}")]
        public async Task<ActionResult> StartGame(
            [SwaggerParameter(Description = "Insert 1 to start the game.")] int key,
            [SwaggerParameter(Description = "Insert your nickname.")] string playerName)
        {
            var players = await _service.ValidateAndReturnPlayers(key, playerName);

            if (players == null || players.Count == 0) return BadRequest(_message.StartGameError());

            // Starting new game

            await _service.StartNewGame(players);

            return Ok(_message.StartGameSuccess(players[1].Name));
        }
    }
}
