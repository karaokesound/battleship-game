using BattleshipGame.API.Services;
using BattleshipGame.Data.Entities;
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

        [HttpPost("{startGame}/{playerOneName}")]
        public async Task<ActionResult> GenerateBoards(int startGame, string playerOneName)
        {
            // Method firstly checks if any fields are generated from previous games. If yes, it deletes all fields.
            // Then player 2 is randomly searched from database and two game boards are created and added to the database
            // so as to manipulate them during a game.

            _fieldRepository.DeleteAllFields();
            await _fieldRepository.SaveChangesAsync();

            Player? player2 = await _playersRepository.GetRandomPlayerAsync(playerOneName);
            if (player2 == null) return NotFound();

            // Create game boards and add them to the database

            var board1 = new GameCore(10, 10, 12, playerOneName, _validation, _generatingService);
            var board2 = new GameCore(10, 10, 12, player2.Name, _validation, _generatingService);

            foreach (var field in board1.Fields)
            {
               await _fieldRepository.AddFieldAsync(field.X, field.Y, field.ShipSize, field.IsEmpty, field.IsHitted, field.IsValid,
                    playerOneName);
            }

            foreach (var field in board2.Fields)
            {
                await _fieldRepository.AddFieldAsync(field.X, field.Y, field.ShipSize, field.IsEmpty, field.IsHitted, field.IsValid,
                    player2.Name);
            }

            await _fieldRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult> DisplayGameBoards()
        {

        }
    }
}
