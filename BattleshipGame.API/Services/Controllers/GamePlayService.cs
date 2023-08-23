using AutoMapper;
using BattleshipGame.API.Models.Game;
using BattleshipGame.API.Services.Repositories;
using BattleshipGame.Data.Entities;
using BattleshipGame.Logic.Services;

namespace BattleshipGame.API.Services.Controllers
{
    public class GamePlayService : IGamePlayService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly iFieldRepository _fieldRepository;
        private readonly IValidationService _validation;
        private readonly IMessageService _message;
        private readonly IGeneratingService _generatingService;
        private readonly IMapper _mapper;

        public GamePlayService(IGameRepository gameRepository,
            IPlayersRepository playersRepository,
            iFieldRepository fieldRepository,
            IValidationService validation,
            IMessageService message,
            IGeneratingService generatingService,
            IMapper mapper)
        {
            _gameRepository = gameRepository;
            _playersRepository = playersRepository;
            _fieldRepository = fieldRepository;
            _validation = validation;
            _message = message;
            _generatingService = generatingService;
            _mapper = mapper;
        }

        public async Task<List<PlayerEntity>> GetPlayers()
        {
            List<PlayerEntity> players = new List<PlayerEntity>();

            var playersIds = await _gameRepository.GetPlayersIds();
            var player1 = await _playersRepository.GetPlayerAsync(playersIds[0]);
            var player2 = await _playersRepository.GetPlayerAsync(playersIds[1]);

            players.Add(player1);
            players.Add(player2);

            return players;
        }

        public async Task<string> PlayersAndCoordsNullCheck(string playerName, string coordinates)
        {
            var players = await GetPlayers();

            if (string.IsNullOrEmpty(coordinates) || players[0] == null || players[1] == null || players[0].Name != playerName)
            {
                return _message.ShootError(players[1].Name, "0");
            }

            return "";
        }

        public async Task<List<string>> UpdatePlayerFields(string playerName, string coordinates)
        {
            var players = await GetPlayers();

            List<int> validCoordsId = _validation.ValidateCoordsFormatAndReturnId(coordinates);
            List<FieldEntity> dbOpponentCoords = await _fieldRepository.GetInsertedFields(validCoordsId, players[1].Name);

            List<string> hittedShipsCoords = new List<string>();

            if (dbOpponentCoords.Any())
            {
                foreach (var coord in dbOpponentCoords)
                {
                    if (!coord.IsEmpty)
                    {
                        coord.IsHitted = true;
                        hittedShipsCoords.Add($"{coord.X}, {coord.Y}");

                        // logika sprawdzenia, czy statek został zatopiony
                    }

                    coord.IsHitted = true;
                }
            }

            // Changing the flag

            players[0].CanShoot = true ? false : true;
            players[1].CanShoot = false ? false : true;

            // Database updating

            _fieldRepository.UpdateFields(dbOpponentCoords);
            await _fieldRepository.SaveChangesAsync();
            await _playersRepository.SaveChangesAsync();

            return hittedShipsCoords;
        }

        public async Task<List<string>> SetRandomShootAndUpdateFields()
        {
            var players = await GetPlayers();

            // Set 3 random coords (computer move)

            List<int> randomCoordinates = _generatingService.GenerateRandomCoordinates(3);
            List<FieldEntity> dbOpponentCoords = await _fieldRepository.GetInsertedFields(randomCoordinates, players[0].Name);

            List<string> hittedShipsCoords = new List<string>();

            foreach (var coord in dbOpponentCoords)
            {
                if (!coord.IsEmpty)
                {
                    coord.IsHitted = true;
                    hittedShipsCoords.Add($"(X, Y) : ({coord.X}, {coord.Y})");
                }

                coord.IsHitted = true;
            }

            // Changing the flag

            players[1].CanShoot = true ? false : true;
            players[0].CanShoot = false ? false : true;

            // Database updating

            _fieldRepository.UpdateFields(dbOpponentCoords);
            await _fieldRepository.SaveChangesAsync();
            await _playersRepository.SaveChangesAsync();

            return hittedShipsCoords;
        }

        public async Task<string> RefreshGameBoard()
        {
            var players = await GetPlayers();

            var playerFields = await _fieldRepository.GetPlayerFieldsAsync(players[0].Name);
            var mappedPlayerFields = new List<Field>();

            foreach (var field in playerFields)
            {
                mappedPlayerFields.Add(_mapper.Map<Field>(field));
            }

            var gameBoard = _generatingService.DisplayGameBoard(mappedPlayerFields, 10, 10);

            return gameBoard;
        }
        public async Task<string> FlagCheck(int value)
        {
            var players = await GetPlayers();

            PlayerEntity selectedPlayer = null;
            PlayerEntity secondPlayer = null;

            // Player

            if (value == 0)
            {
                selectedPlayer = players[0];
                secondPlayer = players[1];
            }

            // Opponent

            if (value == 1)
            {
                selectedPlayer = players[1];
                secondPlayer = players[0];
            }

            if (selectedPlayer == null || secondPlayer == null)
            {
                return _message.PlayerNotFoundMessage();
            }

            string message = "";

            if (!selectedPlayer.CanShoot) message = $"Sorry! This operations can't be done. Now it's {secondPlayer.Name} turn";

            return message;
        }

        public async Task<List<string>> CoordinatesCheck(string coordinates)
        {
            var players = await GetPlayers();

            List<int> validCoordsId = _validation.ValidateCoordsFormatAndReturnId(coordinates);
            List<FieldEntity> dbOpponentCoords = await _fieldRepository.GetInsertedFields(validCoordsId, players[1].Name);

            var usedFields = _validation.ValidateIfFieldsWereHit(dbOpponentCoords);
            if (usedFields.Count > 0) return usedFields;

            return usedFields; // Empty list
        }

        public async Task<List<string>> GetAllHitFields(string playerName)
        {
            var fields = await _fieldRepository.GetHitFields(playerName);

            return fields;
        }
    }
}
