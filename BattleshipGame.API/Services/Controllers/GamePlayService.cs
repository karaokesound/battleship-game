using AutoMapper;
using BattleshipGame.API.Services.Repositories;
using BattleshipGame.Data.Entities;
using BattleshipGame.Logic.Services;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Model;
using System.ComponentModel;
using System.Numerics;

namespace BattleshipGame.API.Services.Controllers
{
    public class GamePlayService : IGamePlayService
    {
        private readonly IGameRepository _gameRepository;
        private readonly IPlayersRepository _playersRepository;
        private readonly iFieldRepository _fieldRepository;
        private readonly IGeneratingService _generatingService;
        private readonly IValidationService _validation;
        private readonly IMapper _mapper;
        private readonly IMessageService _message;

        public GamePlayService(IGameRepository gameRepository,
            IPlayersRepository playersRepository,
            iFieldRepository fieldRepository,
            IGeneratingService generatingService,
            IValidationService validation,
            IMapper mapper,
            IMessageService message)
        {
            _gameRepository = gameRepository;
            _playersRepository = playersRepository;
            _fieldRepository = fieldRepository;
            _generatingService = generatingService;
            _validation = validation;
            _mapper = mapper;
            _message = message;
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

        public async Task<string> ValidatePlayersAndCoordinates(string playerName, string coordinates)
        {
            var playersIds = await _gameRepository.GetPlayersIds();
            var player = await _playersRepository.GetPlayerAsync(playersIds[0]);
            var opponent = await _playersRepository.GetPlayerAsync(playersIds[1]);

            if (string.IsNullOrEmpty(coordinates) || player == null || playerName != player.Name)
            {
                return _message.ShootError(opponent.Name, "0");
            }

            if (!player.CanShoot) return _message.ShootError(opponent.Name, "1");
            return "";
        }

        public async Task<List<string>> UpdatePlayerFields(string playerName, string coordinates)
        {
            var players = await GetPlayers();

            // Validating coordinates

            List<int> validCoordsId = _validation.CoordinatesValidation(coordinates);

            List<FieldEntity> dbValidCoords = await _fieldRepository.GetInsertedFields(validCoordsId, players[1].Name);

            List<string> hittedShipsCoords = new List<string>();

            if (dbValidCoords.Any())
            {
                foreach (var coord in dbValidCoords)
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

            _fieldRepository.UpdateFields(dbValidCoords);
            await _fieldRepository.SaveChangesAsync();
            await _playersRepository.SaveChangesAsync();

            return hittedShipsCoords;
        }

        public async Task<string> ValidateOpponentTurn()
        {
            var players = await GetPlayers();

            if (players[1] == null)
            {
                return _message.PlayerNotFoundMessage();
            }

            string message = "";

            if (!players[1].CanShoot) message = $"Sorry! This operations can't be done. Now it's {players[0].Name} turn";

            return message;
        }

        public async Task<List<string>> SetRandomShotByOpponent()
        {
            var players = await GetPlayers();

            Random random = new Random();
            int randomX = random.Next(0, 9);
            int randomY = random.Next(0, 9);

            List<FieldEntity> fieldsToUpdate = new List<FieldEntity>();
            List<string> hittedFields = new List<string>();
            FieldEntity field = await _fieldRepository.GetPlayerFieldAsync(players[0].Name, randomX, randomY);

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

            players[1].CanShoot = true ? false : true;
            players[0].CanShoot = false ? false : true;

            await _playersRepository.SaveChangesAsync();

            return hittedFields;
        }
    }
}
