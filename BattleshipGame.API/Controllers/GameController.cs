using AutoMapper;
using BattleshipGame.API.Models.Game;
using BattleshipGame.API.Services;
using BattleshipGame.Data.Entities;
using BattleshipGame.Logic.Logic;
using BattleshipGame.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

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

        private readonly IGameRepository _gameRepository;

        public GameController(IValidationService validation,
            IGeneratingService generatingService,
            iFieldRepository fieldRepository,
            IPlayersRepository playersRepository,
            IMapper mapper,
            IMessageService message,
            IGameRepository gameRepository)
        {
            _validation = validation;
            _generatingService = generatingService;
            _fieldRepository = fieldRepository;
            _playersRepository = playersRepository;
            _mapper = mapper;
            _message = message;
            _gameRepository = gameRepository;
        }

        [SwaggerOperation(Summary = "Generate only the game boards")]
        [HttpPost("{playerName}")]
        public async Task<ActionResult> GenerateBoards(
            [SwaggerParameter(Description = "Insert your nickname")] string playerName)
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

        [SwaggerOperation(Summary = "Display a game board for selected user.")]
        [HttpGet("/board/{playerName}")]
        public async Task<ActionResult> DisplayGameBoard(
            [SwaggerParameter(Description = "Insert your nickname")] string playerName)
        {
            // This method firstly check nicknames of two players who are actually playing and
            // returns game board of player whose nickname is inputted.

            List<string> currPlayersName = await _fieldRepository.GetCurrentPlayersByFieldsAsync();

            if (playerName == null 
                || currPlayersName.Count == 0
                || (playerName != currPlayersName[0] && playerName != currPlayersName[1]))
            {
                return NotFound(_message.GameBoardNotFound());
            }

            var playerFields = await _fieldRepository.GetPlayerFieldsAsync(playerName);

            // Mapping entities to game models

            var mappedPlayerFields = _mapper.Map<List<Field>>(playerFields);

            return Ok(_generatingService.GenerateGameBoard(mappedPlayerFields, 10, 10));
        }

        [SwaggerOperation(Summary = "Start the game.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid key or player name.")]
        [HttpPost("{key}/{playerName}")]
        public async Task<ActionResult> StartGame(
            [SwaggerParameter(Description = "Insert 1 to start the game.")] int key,
            [SwaggerParameter(Description = "Insert your nickname.")] string playerName)
        {
            bool playerExists = await _playersRepository.GetPlayerByNameAsync(playerName) == null ? false : true;

            if (key != 1 || playerName == null || !playerExists) return BadRequest(_message.StartGameError());

            // Generates boards for two players

            await GenerateBoards(playerName);

            PlayerEntity player1 = await _playersRepository.GetPlayerByNameAsync(playerName);
            PlayerEntity? player2 = await _playersRepository.GetRandomPlayerAsync(playerName);

            if (player2 == null) return NotFound();

            var player1Fields = await _fieldRepository.GetPlayerFieldsAsync(playerName);

            var player2Fields = await _fieldRepository.GetPlayerFieldsAsync(player2.Name);

            // Add new Game object to the database

            await _gameRepository.AddNewGameAsync(player1, player2, player1Fields, player2Fields);

            return Ok("Game created successfully!");
        }
        
        [SwaggerOperation(Summary = "Shoots at target coordinates.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Shot was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid coordinates format.")]
        [HttpPost("{playerName}/shoot")]
        public async Task<ActionResult> PlayerShoot(
            [SwaggerParameter(Description = "Name of the player who shoots [your nickname]")] string playerName,
            [SwaggerParameter(Description = "Coordinates in the format (x,y): 0,1 2,1")] string coordinates)

        {
            var players = await _fieldRepository.GetCurrentPlayersByFieldsAsync();
            var opponentPlayer = players[1];

            if (string.IsNullOrEmpty(playerName) || string.IsNullOrEmpty(coordinates))
            {
                return BadRequest("Player name and coordinates are required.");
            }

            string[] coordinatePairs = coordinates.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (coordinatePairs.Length > 5) return BadRequest("Maximum 5 coordinate pairs.");

            List<FieldEntity> fieldsToUpdate = new List<FieldEntity>();

            foreach (string coordinatePair in coordinatePairs)
            {
                string[] coordinatesArray = coordinatePair.Split(',');

                if (coordinatesArray.Length != 2 ||
                    !int.TryParse(coordinatesArray[0], out int x) ||
                    !int.TryParse(coordinatesArray[1], out int y))
                {
                    return BadRequest("Invalid coordinates format.");
                }

                FieldEntity field = await _fieldRepository.GetPlayerFieldAsync(opponentPlayer, x, y);

                if (field != null && (field.IsEmpty || !field.IsEmpty))
                {
                    field.IsHitted = true;
                    fieldsToUpdate.Add(field);
                }
            }

            if (fieldsToUpdate.Any())
            {
                _fieldRepository.UpdateFields(fieldsToUpdate);
                await _fieldRepository.SaveChangesAsync();
            }

            return Ok("Shots fired successfully.");
        }
    }
}
