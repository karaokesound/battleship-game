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

        public GameController(IValidationService validation,
            IGeneratingService generatingService,
            iFieldRepository fieldRepository)
        {
            _validation = validation;
            _generatingService = generatingService;
            _fieldRepository = fieldRepository;
        }

        [HttpGet("{startGame}/{username}")]
        public void GenerateBoards(int startGame, string username)
        {
            var board1 = new GameCore(10, 10, 12, _validation, _generatingService);
            var board2 = new GameCore(10, 10, 12, _validation, _generatingService);

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
