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

        [SwaggerOperation(Summary = "Start the game.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Success.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid key or player name.")]
        [HttpPost("start/{key}/{playerName}")]
        public async Task<ActionResult> StartGame(
            [SwaggerParameter(Description = "Insert 1 to start the game.")] int key,
            [SwaggerParameter(Description = "Insert your nickname.")] string playerName)
        {
            PlayerEntity player1 = await _playersRepository.GetPlayerByNameAsync(playerName);
            PlayerEntity? player2 = await _playersRepository.GetRandomPlayerAsync(playerName);

            if (key != 1 || player1 == null || player2 == null) return BadRequest(_message.StartGameError());

            // Deleting previous game data

            _fieldRepository.DeleteAllFields();
            await _fieldRepository.SaveChangesAsync();
            _gameRepository.DeleteAllGames();
            await _gameRepository.SaveChangesAsync();

            // Forcing that Player1[our] takes first shoot

            player1.CanShoot = true;
            player2.CanShoot = false;
            await _playersRepository.SaveChangesAsync();

            await _gameRepository.AddNewGameAsync(player1, player2);
            await _gameRepository.SaveChangesAsync();

            // Creating game boards and adding them to the database

            var board1 = new GameCore(10, 10, 12, player1.Name, _validation, _generatingService);
            var board2 = new GameCore(10, 10, 12, player2.Name, _validation, _generatingService);

            foreach (var field in board1.Fields)
            {
                await _fieldRepository.AddFieldAsync(field, player1);
            }

            foreach (var field in board2.Fields)
            {
                await _fieldRepository.AddFieldAsync(field, player2);
            }

            await _fieldRepository.SaveChangesAsync();
            return Ok(_message.StartGameSuccess(player2.Name));
        }

        [SwaggerOperation(Summary = "Display a game board for inserted user.")]
        [HttpGet("board/{playerName}")]
        public async Task<ActionResult> DisplayGameBoard(string playerName)
        {
            var playersIds = await _gameRepository.GetPlayersIds();
            var player1 = await _playersRepository.GetPlayerAsync(playersIds[0]);
            var opponent = await _playersRepository.GetPlayerAsync(playersIds[1]);

            if (playerName != player1.Name && playerName != opponent.Name)
            {
                return NotFound(_message.GameBoardNotFound());
            }

            var playerFields = await _fieldRepository.GetPlayerFieldsAsync(playerName);

            // Mapping entities to game models

            var mappedPlayerFields = _mapper.Map<List<Field>>(playerFields);

            return Ok(_generatingService.DisplayGameBoard(mappedPlayerFields, 10, 10));
        }
        
        [SwaggerOperation(Summary = "Shoots at target coordinates.")]
        [SwaggerResponse(StatusCodes.Status200OK, "Shot was successful.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, "Invalid coordinates format.")]
        [HttpPatch("shoot/{playerName}")]
        public async Task<ActionResult> ShootAtCoordinates(
            [SwaggerParameter(Description = "Name of the player who shoots [your nickname]")] string playerName,
            [SwaggerParameter(Description = "Coordinates in the format (x,y): 0,1 2,1")] string coordinates)

        {
            var playersIds = await _gameRepository.GetPlayersIds();
            var player = await _playersRepository.GetPlayerAsync(playersIds[0]);
            var opponent = await _playersRepository.GetPlayerAsync(playersIds[1]);

            if (string.IsNullOrEmpty(coordinates) || player == null || playerName != player.Name)
            {
                return BadRequest(_message.ShootError(opponent.Name, "0"));
            }

            if (!player.CanShoot) return BadRequest(_message.ShootError(opponent.Name, "1"));

            // Validating coordinates

            string[] coordinatePairs = coordinates.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (coordinatePairs.Length > 5) return BadRequest(_message.ShootError(opponent.Name, "2"));

            List<FieldEntity> fieldsToUpdate = new List<FieldEntity>();
            List<string> hitteedFields = new List<string>();

            foreach (string coordinatePair in coordinatePairs)
            {
                string[] coordinatesArray = coordinatePair.Split(',');

                if (coordinatesArray.Length != 2 ||
                    !int.TryParse(coordinatesArray[0], out int x) ||
                    !int.TryParse(coordinatesArray[1], out int y))
                {
                    return BadRequest(_message.ShootError(opponent.Name, "2"));
                }

                FieldEntity field = await _fieldRepository.GetPlayerFieldAsync(opponent.Name, x, y);
                
                if (field != null && field.IsEmpty)
                {
                    field.IsHitted = true;
                    fieldsToUpdate.Add(field);
                }
                else if (field != null && !field.IsEmpty)
                {
                    field.IsHitted = true;
                    fieldsToUpdate.Add(field);
                    hitteedFields.Add($"(X, Y) : ({field.X}, {field.Y})");
                }
            }

            if (fieldsToUpdate.Any())
            {
                _fieldRepository.UpdateFields(fieldsToUpdate);
                await _fieldRepository.SaveChangesAsync();
            }

            // Changing the flag

            player.CanShoot = true ? false : true;
            opponent.CanShoot = false ? false : true;

            await _playersRepository.SaveChangesAsync();

            if (hitteedFields.Count > 0)
            {
                var message = _message.ShotSuccess(hitteedFields.Count);
                var jsonResult = new JsonResult(hitteedFields);

                return Ok(new { Message = message, Data = jsonResult.Value });
            }

            return Ok(_message.ShotMissed());
        }

        [HttpPatch("shoot/opponent")]
        public async Task<ActionResult> RandomShootByOpponent()
        {
            var playersIds = await _gameRepository.GetPlayersIds();
            var player = await _playersRepository.GetPlayerAsync(playersIds[0]);
            var opponent = await _playersRepository.GetPlayerAsync(playersIds[1]);

            if (opponent == null)
            {
                return BadRequest(_message.PlayerNotFoundMessage());
            }

            if (!opponent.CanShoot) return BadRequest($"Sorry! This operations can't be proceed. Now it's {player.Name} turn");

            Random random = new Random();
            int randomX = random.Next(0, 9);
            int randomY = random.Next(0, 9);

            List<FieldEntity> fieldsToUpdate = new List<FieldEntity>();
            List<string> hittedFields = new List<string>();
            FieldEntity field = await _fieldRepository.GetPlayerFieldAsync(player.Name, randomX, randomY);

            if (field != null && field.IsEmpty)
            {
                field.IsHitted = true;
                fieldsToUpdate.Add(field);
            }
            else if (field != null && !field.IsEmpty)
            {
                field.IsHitted = true;
                fieldsToUpdate.Add(field);
                hittedFields.Add($"(X, Y) : ({field.X}, {field.Y})");
            }

            opponent.CanShoot = true ? false : true;
            player.CanShoot = false ? false : true;

            await _playersRepository.SaveChangesAsync();

            if (hittedFields.Count > 0)
            {
                var message = $"{opponent.Name} hit {hittedFields.Count} of your field(s). See details below.";
                var jsonResult = new JsonResult(hittedFields);

                return Ok(new { Message = message, Data = jsonResult.Value });
            }

            await DisplayGameBoard(player.Name);

            return Ok(await DisplayGameBoard(player.Name));
        }
    }
}
