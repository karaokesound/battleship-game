using BattleshipGame.API.Services;
using BattleshipGame.API.Services.Controllers;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameInfoController : ControllerBase
    {
        private readonly IGameInfoService _service;
        private readonly IMessageService _message;

        public GameInfoController(IGameInfoService service, IMessageService message)
        {
            _service = service;
            _message = message;
        }

        [SwaggerOperation(Summary = "Display a game board for inserted user.")]
        [HttpGet("board/{playerName}")]
        public async Task<ActionResult> DisplayGameBoard(string playerName)
        {
            bool validation = await _service.ValidatePlayers(playerName);
            
            if (!validation) return NotFound(_message.GameBoardNotFound());

            return Ok(await _service.DisplayGameBoard(playerName));
        }
    }
}
