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
        private readonly IFieldRepository _fieldRepository;
        private readonly IValidationService _validation;
        private readonly IMessageService _message;
        private readonly IGeneratingService _generatingService;
        private readonly IMapper _mapper;

        public GamePlayService(IGameRepository gameRepository,
            IPlayersRepository playersRepository,
            IFieldRepository fieldRepository,
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

        public async Task<string> PlayersAndCoordsValidation(string playerName, string coordinates)
        {
            var players = await GetPlayers();

            var result = _validation.ValidateCoordsFormatAndReturnId(coordinates);

            if (players[0].SunkenShips == 12) return $"The game is over. {players[0].Name} has won.";
            else if (players[1].SunkenShips == 12) return $"The game is over. {players[1].Name} has won.";

            if (string.IsNullOrEmpty(coordinates) || result.Count == 0
                || players[0] == null || players[1] == null || players[0].Name != playerName)
            {
                return _message.ShootError(players[1].Name, "0");
            }

            return "";
        }

        public async Task<CombinedResponseData> UpdatePlayerFields(string playerName, string coordinates)
        {
            var players = await GetPlayers();

            List<FieldEntity> dbOpponentCoords = new List<FieldEntity>();

            if (playerName == players[0].Name)
            {
                List<int> validCoordsId = _validation.ValidateCoordsFormatAndReturnId(coordinates);
                dbOpponentCoords = await _fieldRepository.GetInsertedFields(validCoordsId, players[1].Name);
            }
            else if (playerName == players[1].Name)
            {
                // Set 3 random coords (computer move)

                List<int> randomCoordinates = _generatingService.GenerateRandomCoordinates(3);
                dbOpponentCoords = await _fieldRepository.GetInsertedFields(randomCoordinates, players[0].Name);
            }

            List<string> hitShipsCoords = new List<string>();

            if (dbOpponentCoords.Any())
            {
                bool canTakeAnotherShoot = false;

                foreach (var coord in dbOpponentCoords)
                {
                    if (!coord.IsEmpty)
                    {
                        coord.IsHitted = true;

                        _fieldRepository.UpdateField(coord);
                        await _fieldRepository.SaveChangesAsync();

                        hitShipsCoords.Add($"{coord.X}, {coord.Y}");

                        await CountSunkenShips(coord, players[0]);

                        // Take the next loop
                        canTakeAnotherShoot = true;
                        continue;
                    }

                    coord.IsHitted = true;
                }

                if (!canTakeAnotherShoot)
                {
                    if (playerName == players[0].Name)
                    {
                        players[0].CanShoot = true ? false : true;
                        players[1].CanShoot = false ? false : true;
                    }
                    else if (playerName == players[1].Name)
                    {
                        players[1].CanShoot = true ? false : true;
                        players[0].CanShoot = false ? false : true;
                    }
                }
            }

            // Database updating

            _fieldRepository.UpdateFields(dbOpponentCoords);
            await _fieldRepository.SaveChangesAsync();
            await _playersRepository.SaveChangesAsync();

            return _message.AdjustResponseByPlayerName(players, playerName, hitShipsCoords);
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

            if (!selectedPlayer.CanShoot) return _message.InvalidOperation(players, selectedPlayer.Name);

            return ""; // Empty string
        }

        public async Task<List<string>> GetHitFields(string coordinates)
        {
            var players = await GetPlayers();

            List<int> coordinatesId = _validation.ValidateCoordsFormatAndReturnId(coordinates);

            List<FieldEntity> dbOpponentCoords = await _fieldRepository.GetInsertedFields(coordinatesId, players[1].Name);

            var hitFields = _validation.CheckIfFieldsWereHit(dbOpponentCoords);

            if (hitFields.Count > 0) return hitFields;

            return hitFields; // Empty list
        }

        public async Task<List<string>> GetAllHitFields(string playerName)
        {
            var fields = await _fieldRepository.GetHitFields(playerName);

            return fields;
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

        public async Task CountSunkenShips(FieldEntity coord, PlayerEntity player)
        {
            if (!coord.IsEmpty)
            {
                if (coord.ShipSize == 1)
                {
                    player.SunkenShips += 1;
                }
                if (coord.ShipSize == 2)
                {
                    int amount = await _fieldRepository.GetNumberOfFieldsWithShipId(coord);
                    if (amount == 2)
                    {
                        player.SunkenShips += 1;
                    }
                }
                if (coord.ShipSize == 3)
                {
                    int amount = await _fieldRepository.GetNumberOfFieldsWithShipId(coord);
                    if (amount == 3)
                    {
                        player.SunkenShips += 1;
                    }
                }
            }
        }

        public class CombinedResponseData
        {
            public string Message { get; set; }
            public object JsonData { get; set; }
        }
    }
}
