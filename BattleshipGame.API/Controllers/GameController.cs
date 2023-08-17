using AutoMapper;
using BattleshipGame.API.Models.Game;
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

        private readonly IMapper _mapper;

        private readonly IMessageService _message;

        public GameController(IValidationService validation,
            IGeneratingService generatingService,
            iFieldRepository fieldRepository,
            IPlayersRepository playersRepository,
            IMapper mapper,
            IMessageService message)
        {
            _validation = validation;
            _generatingService = generatingService;
            _fieldRepository = fieldRepository;
            _playersRepository = playersRepository;
            _mapper = mapper;
            _message = message;
        }

        [HttpPost("{startGame}/{playerName}")]
        public async Task<ActionResult> GenerateBoards(int startGame, string playerName)
        {
            // Method firstly checks if any fields are generated from previous games. If yes, it deletes all fields.
            // Then player 2 is randomly searched from database and two game boards are created and added to the database
            // so as to manipulate them during a game.

            _fieldRepository.DeleteAllFields();
            await _fieldRepository.SaveChangesAsync();

            PlayerEntity? player2 = await _playersRepository.GetRandomPlayerAsync(playerName);
            if (player2 == null) return NotFound();

            // Create game boards and add them to the database

            var board1 = new GameCore(10, 10, 12, playerName, _validation, _generatingService);
            var board2 = new GameCore(10, 10, 12, player2.Name, _validation, _generatingService);

            foreach (var field in board1.Fields)
            {
               await _fieldRepository.AddFieldAsync(field.X, field.Y, field.ShipSize, field.IsEmpty, field.IsHitted, field.IsValid,
                    playerName);
            }

            foreach (var field in board2.Fields)
            {
                await _fieldRepository.AddFieldAsync(field.X, field.Y, field.ShipSize, field.IsEmpty, field.IsHitted, field.IsValid,
                    player2.Name);
            }

            await _fieldRepository.SaveChangesAsync();
            return Ok();
        }

        [HttpGet("{playerName}")]
        public async Task<ActionResult> DisplayGameBoard(string playerName)
        {
            // This method firstly check nicknames of two players who are actually playing and
            // returns game board of player who nickname is inputted.

            List<string> currPlayersName = await _fieldRepository.GetCurrentPlayersByFieldsAsync();

            if (playerName == null || (playerName != currPlayersName[0] && playerName != currPlayersName[1]))
            {
                return NotFound(_message.GameBoardNotFound());
            }

            var playerFields = await _fieldRepository.GetPlayerFieldsAsync(playerName);

            // Mapping entities to game models

            var mappedPlayerFields = _mapper.Map<List<Field>>(playerFields);

            return Ok(_generatingService.GenerateGameBoard(mappedPlayerFields, 10, 10));
        }


    }
}
