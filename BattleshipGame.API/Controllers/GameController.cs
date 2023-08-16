using BattleshipGame.API.Services;
using BattleshipGame.Logic.Logic;
using BattleshipGame.Logic.Services;
using Microsoft.AspNetCore.Mvc;

namespace BattleshipGame.API.Controllers
{
    [ApiController]
    [Route("api/game")]
    public class GameController : ControllerBase
    {
        private readonly IValidationService _validation;

        private readonly IGeneratingService _generatingService;

        private readonly iFieldRepository _fieldRepository;

        private readonly IPlayersRepository _playersRepository;

        public GameController(IValidationService validation,
            IGeneratingService generatingService,
            iFieldRepository fieldRepository,
            IPlayersRepository playersRepository)
        {
            _validation = validation;
            _generatingService = generatingService;
            _fieldRepository = fieldRepository;
            _playersRepository = playersRepository;
        }

        [HttpGet("{startGame}/{username}")]
        public void GenerateBoards(int startGame, string username)
        {
            var player = _playersRepository.GetPlayerByNameAsync(username);

            if (player == null) return;

            var board1 = new GameCore(10, 10, 12, username, _validation, _generatingService);
            var board2 = new GameCore(10, 10, 12, username, _validation, _generatingService);

            foreach (var field in board1.Fields)
            {
                //_fieldRepository.AddFieldAsync(field.X, field.Y, field.ShipSize, field.IsEmpty, field.IsHitted, field.IsValid,
                //    Pl);
            }
        }

        //[HttpGet]
        //public async Task<ActionResult> DisplayGameBoards()
        //{
        //}
    }
}
