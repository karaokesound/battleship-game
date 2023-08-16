using BattleshipGame.Logic.Logic;
using BattleshipGame.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IValidationService _validation;

        private readonly IGeneratingService _generatingService;

        public GameController(IValidationService validation,
            IGeneratingService generatingService)
        {
            _validation = validation;
            _generatingService = generatingService;
        }

        [HttpGet("{startGame}")]
        public async Task<ActionResult> GenerateBoards(int startGame)
        {
            var board1 = new GameCore(10, 10, 12, _validation, _generatingService);
            var board2 = new GameCore(10, 10, 12, _validation, _generatingService);
        }

        [HttpGet]
        public async Task<ActionResult> DisplayGameBoards()
        {
        }


    }
}
