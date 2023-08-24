using BattleshipGame.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using static BattleshipGame.API.Services.Controllers.GamePlayService;

namespace BattleshipGame.API.Services
{
    public class MessageService : IMessageService
    {
        public string PlayerNotFoundMessage()
        {
            string message = "Sorry! This user doesn't exist. Check the id or username you've entered and try again.";

            return message;
        }

        public string UserCreatingError()
        {
            string message = "Sorry! Error occurred when creating a new player. Check if you've passed all required data." +
                " Make sure this name is available. You can try with another one.";

            return message;
        }

        public string Delete()
        {
            string message = "Player successfully deleted.";

            return message;
        }

        public string StartGameError()
        {
            string message = "Sorry! Game can't be started because of wrong data. Insert 1 as a key and your player name" +
                " to start the game.";

            return message;
        }

        public string StartGameSuccess(string opponent)
        {
            string message = ($"Game created successfully! Your opponent is {opponent}");

            return message;
        }

        public string GameBoardNotFound()
        {
            string message = "Sorry! Game board for this player name wasn't found. Make sure you've started the game" +
                " and passed correct player name.";

            return message;
        }

        public string ShootError(string opponent, string key)
        {
            string message = "";

            if (key == "0")
            {
                message = "Sorry! Something gone wrong. Check if you've passed valid player name and coordinates and" +
                    " try again.";
            }
            else if (key == "1")
            {
                message = $"You've taken your shoot. Now it's {opponent} turn.";
            }
            else if (key == "2")
            {
                message = "Sorry! You can pass maximum 3 pair of coordinates. Make sure your format is correct and try again.";
            }

            return message;
        }

        public string ShotSuccess(int quantity, List<string> hitShipsCoords)
        {
            string message = "";

            if (hitShipsCoords.Count > 1)
            {
                message = $"Shoot successful! You've hit {quantity} field(s). Let's take another shoot! Hit ships coordinates" +
                    $" are displayed below.";
            }

            if (hitShipsCoords.Count == 1)
            {
                message = $"Shoot successful! You've hit {quantity} field(s). Hit ship was at (X,Y): " +
                    $"({hitShipsCoords[0]}). Let's take another shoot!";
            }

            return message;
        }

        public string InvalidOperation(List<PlayerEntity> players, string playerName)
        {
            string message = "";

            if (players[0].Name == playerName)
            {
                message = $"Sorry! You have taken your shot, now it's {players[1].Name} turn. Take your chance in the next round!";
            }
            else if (players[1].Name == playerName)
            {
                message = $"Sorry! This operation can't be done. It's the {players[0].Name} turn to take the shot.";
            }

            return message;
        }

        public string ShotMissed(List<PlayerEntity> players, string playerName)
        {
            string message = "";

            if (players[0].Name == playerName)
            {
                message = "You took the shot, but unfortunately didn't hit any of the enemy ships. Try again in the next round!";
            }
            else if (players[1].Name == playerName)
            {
                message = "The shot made by the computer was a miss.";
            }
            
            return message;
        }

        public CombinedResponseData AdjustResponseByPlayerName(List<PlayerEntity> players, string playerName,
            List<string> hitShipsCoords)
        {
            CombinedResponseData combinedObject = new CombinedResponseData();

            // Player

            if (players[0].SunkenShips == 12)
            {
                string message = "Congratulate! You've won the game!";
                combinedObject.Message = message;
                return combinedObject;
            }
            else if (players[1].SunkenShips == 12)
            {
                string message = "You've lost! The Opponent destroyed all your ships! Try again by setting new game.";
                combinedObject.Message = message;
                return combinedObject;
            }

            // Opponent (computer)

            if (hitShipsCoords.Count > 0 && playerName == players[1].Name)
            {
                string message = $"{players[1].Name} hit {hitShipsCoords.Count} of your field(s). See details below.";
                var data = new JsonResult(hitShipsCoords);
                combinedObject.Message = message;
                combinedObject.JsonData = data.Value;

                return combinedObject;
            }

            if (hitShipsCoords.Count > 0 && playerName == players[0].Name)
            {
                string message = ShotSuccess(hitShipsCoords.Count, hitShipsCoords);
                var data = new JsonResult(hitShipsCoords);
                combinedObject.Message = message;
                combinedObject.JsonData = data.Value;

                return combinedObject;
            }

            return combinedObject;
        }
    }
}
